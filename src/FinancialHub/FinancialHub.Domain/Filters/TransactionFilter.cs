using System;
using FinancialHub.Domain.Enums;

namespace FinancialHub.Domain.Filters
{
    public class TransactionFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public TransactionType[] Types { get; set; }
        public TransactionStatus[] Status { get; set; }

        public Guid[] Accounts { get; set; }
        public Guid[] Categories { get; set; }
    }
}
