using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Accounts;

namespace FinancialHub.Core.Application.Mappers
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<CreateAccountDto, AccountModel>();
            CreateMap<UpdateAccountDto, AccountModel>();
            CreateMap<AccountModel, AccountDto>().ReverseMap();
        }
    }
}
