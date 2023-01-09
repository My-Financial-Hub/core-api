import { TransactionType } from '../../../../interfaces/transaction';

export type TransactionFilter = {
  types?: TransactionType[],
  startDate?: Date,
  targetDate?: Date,
  categories?: string[],
  accounts?: string[]
};