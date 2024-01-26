namespace FinancialHub.Common.Models
{
    public abstract record class BaseModel
    {
        public Guid? Id { get; private set; }

        protected BaseModel() { }

        protected BaseModel(Guid? id)
        {
            Id = id;
        }
    }
}
