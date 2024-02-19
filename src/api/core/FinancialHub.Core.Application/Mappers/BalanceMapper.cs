using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Balances;

namespace FinancialHub.Core.Application.Mappers
{
    public class BalanceMapper : Profile
    {
        public BalanceMapper()
        {
            CreateMap<CreateBalanceDto, BalanceModel>();
            CreateMap<UpdateBalanceDto, BalanceModel>();

            CreateMap<AccountModel, BalanceAccountDto>();
            CreateMap<BalanceModel, BalanceDto>();
        }
    }
}
