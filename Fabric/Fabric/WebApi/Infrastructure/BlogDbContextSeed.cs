using Demo.Domain.AggregatesModels.UserAggregate;
using Demo.Domain.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Infrastructure
{
    public class BlogDbContextSeed
    {
        public async Task SeedAsync(BlogDbContext context, IWebHostEnvironment env, IOptions<BlogingSettings> settings, ILogger<BlogDbContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(BlogDbContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                try
                {
                    var useCustomizationData = settings.Value.UseCustomizationData;

                    var contentRootPath = env.ContentRootPath;

                    using (context)
                    {
                        context.Database.Migrate();

                        if (useCustomizationData && !context.MyUsers.Any())
                        {
                            context.MyUsers.AddRange(ReadUsersFromFile(contentRootPath, logger));

                            await context.SaveChangesAsync();
                        }

                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                    throw;
                }
                
            });
        }


        private IEnumerable<MyUser> ReadUsersFromFile(string contentRootPath, ILogger<BlogDbContextSeed> logger)
        {
            string csvFileMyUsers = Path.Combine(contentRootPath, "SeedData", "MyUsers.csv");
            
            if (!File.Exists(csvFileMyUsers))
            {
                throw new Exception("MyUsers is null or empty");
            }

            List<MyUser> myUsers = null;
            try
            {
               myUsers = File.ReadAllLines(csvFileMyUsers).Skip(1).Select(c => FromMyUserCSV(c)).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                throw;
            }
            return myUsers;
        }

        private MyUser FromMyUserCSV(string csvLine)
        {
            string[] values = csvLine.Split(',');
            MyUser myUser = new MyUser(values[0], values[1], values[2], values[3]);
            return myUser;
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<BlogDbContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}
