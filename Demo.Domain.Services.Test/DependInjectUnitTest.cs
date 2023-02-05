using Azure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit.Abstractions;
using Xunit.DependencyInjection;

namespace Demo.Domain.Services.Test
{
    public class DependInjectUnitTest
    {
        private readonly ITestOutputHelper? _testOut;
        private readonly IConfiguration _configuration;

        public DependInjectUnitTest(ITestOutputHelperAccessor testOutputHelperAccessor, IConfiguration configuration)
        {
            _testOut = testOutputHelperAccessor.Output;
            _configuration = configuration;
        }

        [Fact]
        public void GetConfiguration_OK()
        {
            var data = _configuration["ConnectionString"];
            _testOut.WriteLine($"configuration[\"ConnectionString\"] = {data}");
            Assert.NotNull(data);
        }
    }
}