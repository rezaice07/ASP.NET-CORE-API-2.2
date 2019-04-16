using AutoMapper;
using SamrtCRM.Data.Models;
using SmartCRM.Api.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCRM.Api.Infrastructures.AutoMapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<User, UserRegistrationViewModel>().ReverseMap();
        }
    }
}
