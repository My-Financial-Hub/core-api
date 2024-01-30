using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Accounts;

namespace FinancialHub.Core.Application.Mappers
{
    public class FinancialHubAccountMapper : Profile
    {
        public FinancialHubAccountMapper()
        {
            CreateMap<CreateAccountDto, AccountModel>();
            CreateMap<UpdateAccountDto, AccountModel>();
            CreateMap<AccountModel, AccountDto>();
        }
    }
}
