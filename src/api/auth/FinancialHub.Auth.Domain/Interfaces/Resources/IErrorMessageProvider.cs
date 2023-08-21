namespace FinancialHub.Auth.Domain.Interfaces.Resources
{
    public interface IErrorMessageProvider
    {
        string? Required { get; }
        string? Invalid { get; }
        string? MinLength { get; }
        string? ConfirmPassword { get; }
        string? MaxLength { get; }
    }
}
