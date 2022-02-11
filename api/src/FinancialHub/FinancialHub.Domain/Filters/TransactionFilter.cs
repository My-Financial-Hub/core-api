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

        //TODO: get a better way to add/get array queries
        public Guid[] Accounts { get; set; }
        public Guid[] Categories { get; set; }

        //public string Accounts { get; set; }
        //public string Categories { get; set; }

        //public Guid[] AccountsIds
        //{
        //    get
        //    {
        //        return this.GetIds(Accounts);
        //    }
        //}
        //public Guid[] CategoriesId
        //{
        //    get
        //    {
        //        return this.GetIds(Categories);
        //    }
        //}

        //public Guid[] GetIds(string value)
        //{
        //    var arr = value?.Split(',') ?? Array.Empty<string>();
        //    var res = new Guid[arr.Length];

        //    for (var i = 0; i < arr.Length; i++)
        //    {
        //        res[i] = new Guid(arr[i]);
        //    }

        //    return res;
        //}
    }
}
