import { useEffect, useState } from 'react';

import { UseDeleteTransaction, UseGetTransactions } from '../../../hooks/transactions-hooks';

import { Transaction } from '../../../interfaces/transaction';

import TransactionListItem from './item/transaction-list-item';
import Loading from '../../../../commom/components/loading/loading';
import { useApisContext } from '../../../contexts/api-context';

interface ITransactionListProps{
  onSelect?: (transaction: Transaction) => void
}

export default function TransactionList({ onSelect }: ITransactionListProps) {
  const [transactions, setTransations] = useState<Transaction[]>([]);
  const [isLoading, setLoading] = useState<boolean>(true);
  const { transactionsApi } = useApisContext();

  useEffect(
    () => {
      const getTransactions = async function () {
        setLoading(true);

        const result = await UseGetTransactions(transactionsApi);
        setTransations(result);

        setLoading(false);
      };

      getTransactions();
    },
    [transactionsApi]
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