using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Balances;

namespace FinancialHub.Core.Application.Mappers
{
    public class BalanceMapper : Profile
    {
        public BalanceMapper()
        {
            CreateMap<CreateBalanceDto, AccountModel>();
            CreateMap<UpdateBalanceDto, AccountModel>();
            CreateMap<BalanceModel, BalanceDto>().ReverseMap();
        }
    }
}
