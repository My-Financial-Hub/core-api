import { Account } from './account';
import { Category } from './category';

export interface Transaction{
  id?: string,
  description: string,
  amount: number,

  finishDate: string,
  targetDate: string,

  accountId: string,
  account?: Account,
  
  categoryId: string,
  category?: Category,

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

  finishDate: new Date().toISOString().split('T')[0],
  targetDate: new Date().toISOString().split('T')[0],

  accountId: '',
  account: undefined,
  
  categoryId: '',
  category: undefined,

  isActive: true,

  type : 1,
  status : 0 
};