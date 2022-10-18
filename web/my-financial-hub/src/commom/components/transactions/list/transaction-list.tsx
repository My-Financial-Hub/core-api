import { useEffect, useState } from 'react';

import { useApisContext } from '../../../contexts/api-context';
import { useGetTransactions } from '../../../hooks/transactions-hooks';

import { Transaction} from '../../../interfaces/transaction';

import TransactionListItem from './item/transaction-list-item';
import Loading from '../../../../commom/components/loading/loading';

type TransactionListProps = {
  onSelect?: (transaction: Transaction) => void
};

export default function TransactionList({onSelect}: TransactionListProps){
  const [transactions,setTransations] = useState<Transaction[]>([]);
  const [isLoading,setLoading] = useState<boolean>(true);
  
  const { transactionsApi } = useApisContext();
  
  const getTransactions = async ()=>{
    setLoading(true);

    const trans = await useGetTransactions(transactionsApi);
    setTransations(trans);

    setLoading(false);
  };

  const selectTransaction = function(transaction: Transaction){
    onSelect?.(transaction);
  };
  
  useEffect(
    ()=>{
      getTransactions();
    },[]
  );

  if(isLoading){
    return (
      <Loading />
    );
  }else{
    if(transactions.length > 0){
      return (
        <ul>
          {
            transactions?.map(
              transaction =>(
                <TransactionListItem key={transaction.id} transaction={transaction} onSelect={selectTransaction}/>
              )
            )
          }
        </ul>
      );
    }else{
      return (
        <p>No transactions</p>
      );
    }
  }
}