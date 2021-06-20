using AutoMapper;
using Demo.Domain.AggregatesModels.UserAggregate;
using WebApi.Application.ViewModels;

namespace WebApi.Infrastructure
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
