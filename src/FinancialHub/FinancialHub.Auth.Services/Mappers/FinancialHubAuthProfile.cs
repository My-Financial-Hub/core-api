using AutoMapper;
using FinancialHub.Auth.Domain.Entities;
using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Services.Mappers
{
    internal class FinancialHubAuthProfile : Profile
    {
        public FinancialHubAuthProfile()
        {
            this.CreateMap<UserEntity,UserModel>().ReverseMap();
        }
    }
}
