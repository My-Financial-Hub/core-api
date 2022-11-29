import { useEffect, useState } from 'react';
import { useApisContext } from '../contexts/api-context';

import TransactionApi from '../http/transaction-api';
import { Transaction } from '../interfaces/transaction';

export async function useCreateTransaction(transaction: Transaction, api: TransactionApi) : Promise<Transaction> {
  try {
    const result = await api.PostAsync(transaction);
    return result.data;
  } catch (error) {
    console.error(error);
    return Promise.reject();
  }
}

export async function useUpdateTransaction(transaction: Transaction, api: TransactionApi) {
  try {
    if(transaction.id){
      await api.PutAsync(transaction.id,transaction);
    }
  } catch (error) {
    console.error(error);
  }
}

export type TransactionFetchState = {
  transactions: Transaction[],
  isLoading: boolean,
  hasError: boolean
}

/**
 * @deprecated use UseGetTransactions
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

  return { transactions, isLoading, hasError };
}

export async function UseGetTransactions(transactionsApi: TransactionApi): Promise<Transaction[]> {
  try {
    const transactionsResult = await transactionsApi.GetAllAsync();
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