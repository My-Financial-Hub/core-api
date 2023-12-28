namespace FinancialHub.Core.Domain.Interfaces.Resources
{
    public interface IErrorMessageProvider
    {
        string NotFoundMessage(string name, Guid id);
        string UpdateFailedMessage(string name, Guid id);
    }
}
