using AutoMapper;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Filters;
using FinancialHub.Domain.Queries;

namespace FinancialHub.Services.Adapters
{
    public class FinancialHubAutoMapperProfile : Profile
    {
        public FinancialHubAutoMapperProfile()
        {
            //Entities
            CreateMap<AccountEntity,AccountModel>().ReverseMap();
            CreateMap<CategoryEntity,CategoryModel>().ReverseMap();
            CreateMap<TransactionEntity, TransactionModel>().ReverseMap();

            //Queries
            CreateMap<TransactionFilter, TransactionQuery>().ReverseMap();
        }
    }
}
