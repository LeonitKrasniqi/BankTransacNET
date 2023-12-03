using AccountManagmentSystemAPI.Model.Domain;
using AccountManagmentSystemAPI.Model.Dto;
using AutoMapper;

namespace AccountManagmentSystemAPI.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<AddAccountRequestDto,Account>().ReverseMap();
            
        }
    }
}
