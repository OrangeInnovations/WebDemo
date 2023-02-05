using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Domain.AggregatesModels.UserAggregate;
using Demo.Domain.Services.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace FunctionApp
{
    public class UserFunction
    {
        private readonly ILogger<UserFunction> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMapper mapper;
        private readonly IMyUserRepository myUserRepository;

        public UserFunction(ILogger<UserFunction> log, IConfiguration configuration, IMapper mapper, IMyUserRepository myUserRepository)
        { 
            _logger = log;
            _configuration = configuration;
            this.mapper = mapper;
            this.myUserRepository = myUserRepository;
        }

        [FunctionName("AllUsers")]
        [OpenApiOperation(operationId: "AllUsers", tags: new[] { "AllUsers" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: " application/json", bodyType: typeof(List<MyUserVM>), Description = "The OK response")]
        public async Task<IActionResult> GetAllUsers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            var myusers = await myUserRepository.GetAllAsync();
            var list = mapper.Map<List<MyUserVM>>(myusers);

            return new OkObjectResult(list);
        }

        [FunctionName("Test")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            string sqlcon = _configuration["ConnectionString"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}

