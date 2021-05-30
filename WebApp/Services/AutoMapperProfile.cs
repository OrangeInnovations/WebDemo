using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            CreateSampleDbModelsMappings();


        }

        private void CreateSampleDbModelsMappings()
        {
            //CreateMap<CustomerMaster, CustomerMasterVm>();

            //CreateMap<FrequencyMaster, FrequencyMasterVm>();
            //CreateMap<UsecaseMaster, UsecaseMasterVm>();
        }
    }
}
