import { Account, defaultAccount } from './account';
import { Category, defaultCategory } from './category';

export interface Transaction{
  id?: string,
  description: string,
  amount: number,

  finishDate: Date,
  targetDate: Date,

  accountId: string,
  account: Account,
  
  categoryId: string,
  category: Category,

  isActive: boolean,

  type : TransactionType,
  status : TransactionStatus
}

export enum TransactionType{
  Earn = 1,
  Expense = 2
}

export enum TransactionStatus
{
  NotCommitted = 0,
  Committed = 1
}

export const defaultTransaction: Transaction = {
  description: '',
  amount: 1,

  finishDate: new Date(),
  targetDate: new Date(),

  accountId: '',
  account: defaultAccount,
  
  categoryId: '',
  category: defaultCategory,

  isActive: true,

  type : 1,
  status : 0 
};