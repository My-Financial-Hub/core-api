import { Dispatch, SetStateAction, useEffect, useState } from 'react';
import { TransactionFilter } from '../components/transactions/list/types/transaction-filter';
import { useApisContext } from '../contexts/api-context';

import TransactionApi, { FetchTransactions } from '../http/transaction-api';
import { Transaction } from '../interfaces/transaction';

export async function UseCreateTransaction(transaction: Transaction, api: TransactionApi) : Promise<Transaction> {
  try {
    const result = await api.PostAsync(transaction);
    return result.data;
  } catch (error) {
    console.error(error);
    return Promise.reject();
  }
}

export async function UseUpdateTransaction(transaction: Transaction, api: TransactionApi) {
  try {
    if(transaction.id){
      await api.PutAsync(transaction.id,transaction);
    }
  } catch (error) {
    console.error(error);
  }
}

export type TransactionFetchState = {
  transactions: [Transaction[], Dispatch<SetStateAction<Transaction[]>>],
  isLoading: boolean,
  hasError: boolean
}

/**
 * @deprecated use UseGetTransactions (this hook needs improvement)
 */
export function useFetchTransactions(): TransactionFetchState{
  const [transactions, setTransations] = useState<Transaction[]>([]);
  const [isLoading, setLoading] = useState<boolean>(true);
  const [hasError,setHasError] = useState<boolean>(false);
  const { transactionsApi } = useApisContext();
  
  useEffect(
    () => {
      const getTransactions = async function () {
        setLoading(true);
        try {
          const transactionsResult = await transactionsApi.GetAllAsync();
          setTransations(transactionsResult.data);
        } catch (error) {
          console.error(error);
          setHasError(true);
          return Promise.reject();
        }    
    
        setLoading(false);
      };

      getTransactions();
    },
    [transactionsApi]
  );

  return { 
    transactions: [transactions, setTransations], 
    isLoading,
    hasError
  };
}

export async function UseGetTransactions(filter?: TransactionFilter): Promise<Transaction[]> {
  try {
    const transactionsResult = await FetchTransactions(filter);
    return transactionsResult.data;
  } catch (error) {
    console.error(error);
    return Promise.reject();
  }
}

export async function UseDeleteTransaction(id: string, transactionsApi: TransactionApi) {
  try {
    await transactionsApi.DeleteAsync(id);
  } catch (error) {
    console.error(error);
  }
}