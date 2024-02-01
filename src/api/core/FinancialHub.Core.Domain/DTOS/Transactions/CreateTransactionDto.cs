﻿using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Domain.DTOS.Transactions
{
    public class CreateTransactionDto
    {
        public string Description { get; private set; }
        public decimal Amount { get; private set; }

        public DateTimeOffset TargetDate { get; private set; }
        public DateTimeOffset FinishDate { get; private set; }

        public Guid BalanceId { get; private set; }

        public Guid CategoryId { get; private set; }

        public TransactionStatus Status { get; private set; }
        public TransactionType Type { get; private set; }
    }
}
