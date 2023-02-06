using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApi.Test.Helpers;
using Xunit.Abstractions;

namespace WebApi.Test.Controllers
{
    public class WeatherForecastTest: IClassFixture<ApiTestClassFixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly ApiTestClassFixture _fixture;
        public WeatherForecastTest(ApiTestClassFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
        }

        [Fact]
        public async Task GetWeatherForecast_Success()
        {
            var httpClient = _fixture.Client;
            
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response=await httpClient.GetAsync("api/WeatherForecast");
            if(response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                _output.WriteLine($"Run GetWeatherForecast , result={data}");
            }
        }
    }
}
