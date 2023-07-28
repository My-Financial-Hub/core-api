using AutoMapper;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Filters;
using FinancialHub.Domain.Queries;

namespace FinancialHub.Services.Mappers
{
    public class FinancialHubAutoMapperProfile : Profile
    {
        public FinancialHubAutoMapperProfile()
        {
            //Entities
            CreateMap<AccountEntity,AccountModel>().ReverseMap();
            CreateMap<CategoryEntity,CategoryModel>().ReverseMap();
            CreateMap<TransactionEntity, TransactionModel>().ReverseMap();
            CreateMap<BalanceEntity, BalanceModel>().ReverseMap();

            //Queries
            CreateMap<TransactionFilter, TransactionQuery>().ReverseMap();
        }
    }
}
