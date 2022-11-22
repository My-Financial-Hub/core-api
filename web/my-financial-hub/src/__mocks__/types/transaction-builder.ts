import { faker } from '@faker-js/faker';

import { Account } from '../../commom/interfaces/account';
import { Category } from '../../commom/interfaces/category';
import { Transaction, TransactionStatus, TransactionType } from '../../commom/interfaces/transaction';

import { CreateAccount } from './account-builder';
import { CreateCategory } from './category-builder';

type TransactionBuilderArgs = {
  id?: string, 
  description?: string,
  amount?: number,

  finishDate?: string,
  targetDate?: string,

  accountId?: string,
  account?: Account,

  categoryId?: string,
  category?: Category,

  isActive?: boolean,

  type?: TransactionType,
  status?: TransactionStatus
}

export function CreateTransaction(args? :TransactionBuilderArgs) : Transaction {
  return {
    id: args?.id?? faker.datatype.uuid(),
    description: args?.description?? faker.lorem.words(),
    amount: args?.amount?? parseFloat(faker.finance.amount()),

    finishDate: args?.finishDate?? faker.date.soon().toISOString().split('T')[0],
    targetDate: args?.finishDate?? faker.date.soon().toISOString().split('T')[0],

    accountId: args?.accountId?? faker.datatype.uuid(),
    account: args?.account?? CreateAccount(),

    categoryId: args?.categoryId?? faker.datatype.uuid(),
    category: args?.category?? CreateCategory(),

    status: args?.status?? faker.datatype.number({min: 0, max: 1}),
    type: args?.type?? faker.datatype.number({min: 1, max: 2}),

    isActive:     args?.isActive    ?? faker.datatype.boolean()
  };
}

export function CreateTransactions(args?:TransactionBuilderArgs) : Transaction[]{
  const count = faker.datatype.number(
    {
      min: 1,
      max: 10
    }
  );

  return Array.from(
    { length: count }, 
    () => CreateTransaction(args)
  );
}