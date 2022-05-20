using System;
using System.Linq;
using System.Collections.Generic;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Enums;

namespace FinancialHub.Domain.Queries
{
    public class TransactionQuery
    {
        [Obsolete("Not Used yet")]
        public string UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Guid[] Balances { get; set; }
        public Guid[] Categories { get; set; }

        public TransactionType[] Types { get; set; }
        public TransactionStatus[] Status { get; set; }

        //public Guid[] AccountsIds { get; set; }
        //public Guid[] CategoriesIds { get; set; }

        public Func<TransactionEntity, bool> Query()
        {
            //TODO: compare performance
            return this.QueryFuncs();
        }

        private Func<TransactionEntity, bool> QueryFuncs()
        {
            var queries = new List<Func<TransactionEntity, bool>>();

            queries.Add((ent) => ent.IsActive);

            if (StartDate != null)
            {
                if (EndDate == null)
                {
                    StartDate = new DateTime(StartDate.Value.Year, StartDate.Value.Month,1);
                    EndDate = StartDate.Value.AddMonths(2).AddSeconds(-1);
                }
                queries.Add((ent) => ent.TargetDate >= StartDate && ent.TargetDate <= EndDate);
            }

            if (Balances.Length > 0)
            {
                queries.Add((ent) => Balances.Contains(ent.BalanceId));
            }

            if (Categories.Length > 0)
            {
                queries.Add((ent) => Categories.Contains(ent.CategoryId));
            }

            if (Types.Length > 0)
            {
                queries.Add((ent) => Types.Contains(ent.Type));
            }

            if (Status.Length > 0)
            {
                queries.Add((ent) => Status.Contains(ent.Status));
            }

            return (ent) => queries.All(query => query(ent));
        }
    }
}
