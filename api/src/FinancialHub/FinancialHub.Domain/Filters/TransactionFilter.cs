using System;

namespace FinancialHub.Domain.Filters
{
    public class TransactionFilter
    {
        public int? Month { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        //TODO: get a better way to filter
        //public Guid[] Accounts { get; set; }
        //public Guid[] Categories { get; set; }
    }
}
