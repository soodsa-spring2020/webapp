using AutoMapper;

namespace csye6225.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<AccountModel, AccountResponse>();
        }
    }
}