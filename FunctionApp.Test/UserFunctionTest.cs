using FunctionApp.Test.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace FunctionApp.Test
{
    public class UserFunctionTest: IClassFixture<AzureFunctionFixture>
    {
        private readonly AzureFunctionFixture _fixture;
        private readonly ITestOutputHelper _output;
        private HttpClient _httpClient;
       
        public UserFunctionTest(AzureFunctionFixture fixture, ITestOutputHelper testOutputHelper)
        {
            this._fixture = fixture;
            this._output = testOutputHelper;

            _httpClient = fixture.HttpClient;
        }

        [Fact]
        public async Task GetAllUsers_OK()
        {
            string url = "AllUsers";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                _output.WriteLine($"Run GetAllUsers , result={data}");
            }
        }
    }
}