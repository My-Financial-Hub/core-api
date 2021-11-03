using System;

namespace FinancialHub.Domain.Model
{
    public abstract class BaseModel
    {
        public Guid? Id { get; set; }
    }
}
