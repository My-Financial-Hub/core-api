import { faker } from '@faker-js/faker';
import { Transaction, TransactionStatus, TransactionType } from '../../commom/interfaces/transaction';

type TransactionBuilderArgs = {
  id?: string, 
  description: string,
  amount: number,

  finishDate: Date,
  targetDate: Date,

  accountId: string,
  categoryId: string,

  isActive: boolean,

  type : TransactionType,
  status : TransactionStatus
}

export function CreateTransaction(args? :TransactionBuilderArgs) : Transaction {
  return {
    id: args?.id?? faker.datatype.uuid(),
    description: args?.description?? faker.lorem.words(),
    amount: args?.amount?? parseFloat(faker.finance.amount()),

    finishDate: args?.finishDate?? faker.date.soon(),
    targetDate: args?.finishDate?? faker.date.soon(),

    accountId: args?.accountId?? faker.datatype.uuid(),
    categoryId: args?.categoryId?? faker.datatype.uuid(),

    status: args?.status?? faker.datatype.number({min: 0, max: 1}),
    type: args?.type?? faker.datatype.number({min: 1, max: 2}),

    isActive:     args?.isActive    ?? faker.datatype.boolean()
  };
}

export function CreateTransactions(args:TransactionBuilderArgs) : Transaction[]{
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