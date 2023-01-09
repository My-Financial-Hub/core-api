import { useEffect, useState } from 'react';

import { UseDeleteTransaction, UseGetTransactions } from '../../../hooks/transactions-hooks';
import { useApisContext } from '../../../contexts/api-context';

import { Transaction } from '../../../interfaces/transaction';
import { TransactionFilter } from './types/transaction-filter';

import TransactionListItem from './item/transaction-list-item';
import Loading from '../../../../commom/components/loading/loading';

interface ITransactionListProps{
  filter?: TransactionFilter,
  onSelect?: (transaction: Transaction) => void
}

//TODO: use hook for states : https://youtu.be/bGzanfKVFeU?t=946

export default function TransactionList({ filter, onSelect }: ITransactionListProps) {
  const [transactions, setTransations] = useState<Transaction[]>([]);
  const [isLoading, setLoading] = useState<boolean>(true);
  const { transactionsApi } = useApisContext();
  
  useEffect(
    () => {
      const getTransactions = async function () {
        setLoading(true);
    
        const result = await UseGetTransactions(filter);
        setTransations(result);
    
        setLoading(false);
      };
      getTransactions();
    },
    [filter]
  );
  
  const selectTransaction = function (transaction: Transaction) {
    onSelect?.(transaction);
  };
  
  const removeTransaction = async function (id?: string) {
    if(id){      
      setLoading(true);

      await UseDeleteTransaction(id, transactionsApi);
      setTransations(transactions.filter(t => t.id != id));

      setLoading(false);
    }
  };

  if(isLoading){
    return (
      <Loading />
    );
  }else{
    if(transactions.length > 0){
      return (
        <ul data-testid="transaction-list">
          {
            transactions?.map(
              transaction =>(
                <TransactionListItem 
                  key={transaction.id} 
                  transaction={transaction} 
                  onSelect={selectTransaction}
                  onRemove={removeTransaction}
                />
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