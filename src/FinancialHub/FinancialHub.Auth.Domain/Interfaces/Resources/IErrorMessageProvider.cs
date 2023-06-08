namespace FinancialHub.Auth.Domain.Interfaces.Resources
{
    public interface IErrorMessageProvider
    {
        string? Required { get; }
        string? Invalid { get; }
        string? MaxLength { get; }
    }
}
