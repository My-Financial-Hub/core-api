﻿using AutoMapper;
using FinancialHub.Auth.Domain.Entities;
using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Infra.Mappers
{
    public class FinancialHubAuthProviderProfile : Profile
    {
        public FinancialHubAuthProviderProfile()
        {
            this.CreateMap<UserEntity,UserModel>().ReverseMap();
        }
    }
}