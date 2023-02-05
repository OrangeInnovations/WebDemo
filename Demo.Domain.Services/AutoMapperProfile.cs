using AutoMapper;
using Demo.Domain.AggregatesModels.UserAggregate;
using Demo.Domain.Services.ViewModels;

namespace Demo.Domain.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMappings();
        }

        private void CreateMappings()
        {
            CreateMyUserMappings();


        }

        private void CreateMyUserMappings()
        {
            CreateMap<MyUser, MyUserVM>();

            
        }
    }
}
