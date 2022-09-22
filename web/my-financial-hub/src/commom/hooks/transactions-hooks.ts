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


export async function useGetTransactions(api: TransactionApi): Promise<Transaction[]> {
  try {
    const transactionsResult = await api.GetAllAsync();
    return transactionsResult.data;
  } catch (error) {
    console.error(error);
    return Promise.reject();
  }
}

export async function useDeleteTransaction(id: string,api: TransactionApi) {
  try {
    await api.DeleteAsync(id);
  } catch (error) {
    console.error(error);
  }
}