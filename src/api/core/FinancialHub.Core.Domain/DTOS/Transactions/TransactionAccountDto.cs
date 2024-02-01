﻿namespace FinancialHub.Core.Domain.DTOS.Transactions
{
    public class TransactionAccountDto
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
    }
}
