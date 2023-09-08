using FinancialHub.Core.Domain.Filters;
using FinancialHub.Core.Domain.Queries;

namespace FinancialHub.Core.Infra.Mappers
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
