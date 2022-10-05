import { useState } from 'react';
import { Account } from '../../../interfaces/account';
import { Category } from '../../../interfaces/category';
import { Transaction , TransactionType, TransactionStatus} from '../../../interfaces/transaction';
import TransactionListItem from './item/transaction-list-item';

const dataTest: Transaction[] = [
  {
    id:'1',
    description: 'descricao',
    amount: 10.00,

    finishDate: new Date(Date.parse('2022-10-04')),
    targetDate: new Date(Date.parse('2022-10-14')),

    accountId: '1',
    account: {
      id: '1',
      name: 'account',
    } as Account,
    
    categoryId: '1',
    category: {
      id: '1',
      name: 'category',
    } as Category,

    isActive: true,

    type : TransactionType.Earn,
    status : TransactionStatus.Committed
  },
  {
    id:'2',
    description: 'descricao',
    amount: 10.00,

    finishDate: new Date(Date.parse('2022-10-04')),
    targetDate: new Date(Date.parse('2022-10-14')),

    accountId: '2',
    account: {
      id: '2',
      name: 'account 2',
      currency: 'R$'
    } as Account,
    
    categoryId: '1',
    category: {
      id: '1',
      name: 'category',
    } as Category,

    isActive: true,

    type : TransactionType.Expense,
    status : TransactionStatus.NotCommitted
  }
];
export default function TransactionList(){
  const [transactions,setTransations] = useState<Transaction[]>(dataTest);
  return (
    <ul>
      {
        transactions?.map(
          transaction =>(
            <TransactionListItem key={transaction.id} transaction={transaction}/>
          )
        )
      }
    </ul>
  );
}