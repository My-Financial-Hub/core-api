namespace FinancialHub.Common.Models
{
    public abstract record class BaseModel
    {
        public Guid? Id { get; set; }
    }
}
