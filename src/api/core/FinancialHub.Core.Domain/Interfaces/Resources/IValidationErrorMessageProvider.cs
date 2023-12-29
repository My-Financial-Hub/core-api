namespace FinancialHub.Core.Domain.Interfaces.Resources
{
    public interface IValidationErrorMessageProvider
    {
        string Required { get; }
        string ExceedMaxLength { get; }
        string GreaterThan { get; }
        string OutOfEnum { get; }
    }
}
