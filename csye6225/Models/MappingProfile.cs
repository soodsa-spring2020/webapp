using System;
using System.Linq;
using AutoMapper;
using csye6225.Common.Enums;

namespace csye6225.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            //Account
            CreateMap<AccountModel, AccountResponse>();

            //Bill
            CreateMap<BillModel, BillResponse>()
            .ForMember(dest => dest.categories, m => m.MapFrom(src => src.categories.Split(',',System.StringSplitOptions.None).ToList()))
            .ForMember(dest => dest.payment_status, m => m.MapFrom(src => Enum.GetName(typeof(PaymentStatusEnum), src.payment_status)));
        
        }
    }
}