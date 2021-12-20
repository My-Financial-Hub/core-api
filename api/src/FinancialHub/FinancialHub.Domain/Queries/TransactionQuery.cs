using FinancialHub.Domain.Entities;
using System;

namespace FinancialHub.Domain.Queries
{
    public class TransactionQuery
    {
        [Obsolete("Not Used yet")]
        public string UserId { get; set; }
        public int? Month { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Func<TransactionEntity, bool> Query()
        {
            Func<TransactionEntity, bool> query;

            if (Month == null && StartDate == null && EndDate == null)
            {
                query = (ent) => true;
            }
            else
            {

                if (Month != null)
                {
                    query = (ent) => ent.TargetDate.Month == Month;
                }
                else
                {
                    query = (ent) => ent.TargetDate >= StartDate && ent.TargetDate <= EndDate;
                }
            }

            return query;
        }
    }
}
