﻿using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Accounts;

namespace FinancialHub.Core.Application.Mappers
{
    internal class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<CreateAccountDto, AccountModel>();
            CreateMap<UpdateAccountDto, AccountModel>();

            CreateMap<AccountBalanceDto, BalanceModel>().ReverseMap();
            CreateMap<AccountDto, AccountModel>().ReverseMap();
        }
    }
}
