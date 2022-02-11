using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FinancialHub.Domain.Queries
{
    public class TransactionQuery
    {
        [Obsolete("Not Used yet")]
        public string UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Guid[] Accounts { get; set; }
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

            if (Accounts.Length > 0)
            {
                queries.Add((ent) => Accounts.Contains(ent.AccountId));
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

        private Func<TransactionEntity, bool> QueryFunc()
        {
            Func<TransactionEntity, bool> query;

            if (EndDate == null)
            {
                query = (ent) => ent.TargetDate == StartDate;
            }
            else
            {
                query = (ent) => ent.TargetDate >= StartDate && ent.TargetDate <= EndDate;
            }

            //TODO: idk how to add it
            if (Accounts.Length > 0)
            {

            }

            if (Categories.Length > 0)
            {

            }

            return query;
        }

        [Obsolete("Not working : exception at Expression.And")]
        private Func<TransactionEntity, bool> QueryExpression()
        {
            //TODO: do a better filter system (IQueryable Results on repository)
            var expressions = new List<Expression<Func<TransactionEntity, bool>>>();
            expressions.Add((ent) => ent.IsActive);

            if (EndDate == null)
            {
                expressions.Add((ent) => ent.TargetDate == StartDate);
            }
            else
            {
                expressions.Add((ent) => ent.TargetDate >= StartDate && ent.TargetDate <= EndDate);
            }

            if (Accounts.Length > 0)
            {
                expressions.Add((ent) => Accounts.Contains(ent.AccountId));
            }

            if (Categories.Length > 0)
            {
                expressions.Add((ent) => Accounts.Contains(ent.CategoryId));
            }

            Expression expression = expressions.First();

            foreach (var exp in expressions.Skip(1))
            {
                expression = Expression.And(expression, exp);
            }

            var parameter = Expression.Parameter(typeof(TransactionEntity), "ent");
            var query = Expression.Lambda<Func<TransactionEntity, bool>>(expression, parameter);

            return query.Compile();
        }
    }
}
