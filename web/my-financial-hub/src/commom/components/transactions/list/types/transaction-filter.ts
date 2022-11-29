export type TransactionFilter = {
  startDate?: Date,
  targetDate?: Date,
  categories?: string[],
  accounts?: string[],
  minAmount?: number,
  maxAmount?: number
};