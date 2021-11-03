using AutoMapper;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Models;

namespace FinancialHub.WebApi
{
    public class FinancialHubAutoMapperProfile : Profile
    {
        public FinancialHubAutoMapperProfile()
        {
            CreateMap<AccountEntity,AccountModel>().ReverseMap();
            CreateMap<CategoryEntity,CategoryModel>().ReverseMap();
        }
    }
}
