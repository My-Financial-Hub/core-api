using FluentValidation.Results;
using static FinancialHub.Common.Results.Errors.ValidationError;

namespace FinancialHub.Core.Application.Extensions
{
    public static class ValidationFailureExtensions
    {
        public static FieldValidationError[] ToFieldValidationError(this List<ValidationFailure> failures)
        {
            return failures
                .GroupBy(x => x.PropertyName)
                .Select(x =>
                    new FieldValidationError(
                        field: x.Key,
                        messages: x.Select(y => y.ErrorMessage).ToArray()
                    )
                ).ToArray();
        }
    }
}
