using AutoMapper;
using Demo.Domain.AggregatesModels.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Application.ViewModels;

namespace WebApp.Services
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
