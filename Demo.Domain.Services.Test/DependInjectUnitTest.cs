using AutoMapper;
using Demo.Domain.AggregatesModels.UserAggregate;
using Demo.Domain.Services.ViewModels;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;
using Xunit.DependencyInjection;

namespace Demo.Domain.Services.Test
{
    public class DependInjectUnitTest
    {
        private readonly ITestOutputHelper? _testOut;
        private readonly IConfiguration _configuration;
        private readonly IMapper mapper;
        private IMyUserRepository myUserRepository;

        public DependInjectUnitTest(ITestOutputHelperAccessor testOutputHelperAccessor, IConfiguration configuration, IMapper mapper, IMyUserRepository myUserRepository)
        {
            _testOut = testOutputHelperAccessor.Output;
            _configuration = configuration;
            this.mapper = mapper;
            this.myUserRepository = myUserRepository;
        }

        [Fact]
        public void GetConfiguration_OK()
        {
            var data = _configuration["ConnectionString"];
            _testOut.WriteLine($"configuration[\"ConnectionString\"] = {data}");
            Assert.NotNull(data);
        }

        [Fact]
        public async Task GetAllUsers_OK()
        {
            var myusers = await myUserRepository.GetAllAsync();
            var list = mapper.Map<List<MyUserVM>>(myusers);

            Assert.NotNull(list);
        }
    }
}