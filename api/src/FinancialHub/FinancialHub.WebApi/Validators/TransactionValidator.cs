using FinancialHub.Domain.Models;
using FinancialHub.WebApi.Resources;
using FluentValidation;

namespace FinancialHub.WebApi.Validators
{
    public class TransactionValidator : AbstractValidator<TransactionModel>
    {
        public TransactionValidator()
        {
            RuleFor(x => x.Description)
                .Length(0, 500)
                .WithMessage(ErrorMessages.ExceedMaxLength);

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage(ErrorMessages.GreaterThan);
        }

        public void CoverageTest()
        {
            if( new System.Random().Next(10) > 8)
            {
                System.Console.WriteLine("a");
            }
            else
            {
                System.Console.WriteLine("b");
            }
        }
    }
}
