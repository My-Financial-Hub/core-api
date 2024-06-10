using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Transactions;

namespace FinancialHub.Core.Application.Mappers
{
    internal class TransactionMapper : Profile
    {
        public TransactionMapper()
        {
            CreateMap<CreateTransactionDto, TransactionModel>();
            CreateMap<UpdateTransactionDto, TransactionModel>();

            CreateMap<TransactionAccountDto, AccountModel>().ReverseMap();
            CreateMap<TransactionBalanceDto, BalanceModel>().ReverseMap();
            CreateMap<TransactionCategoryDto, CategoryModel>().ReverseMap();
            CreateMap<TransactionDto, TransactionModel>().ReverseMap();
        }
    }
}
