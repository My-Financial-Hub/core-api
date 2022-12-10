import { ChangeEvent, FormEvent, useEffect, useState } from 'react';

import { useApisContext } from '../../../../contexts/api-context';
import { UseCreateTransaction, UseUpdateTransaction } from '../../../../hooks/transactions-hooks';

import { defaultTransaction, Transaction, TransactionStatus } from '../../../../interfaces/transaction';

import SelectOption from '../../../forms/form-select/types/select-option';

interface ITransactionFormHook {
  changeField: (event: ChangeEvent<HTMLInputElement|HTMLTextAreaElement>) => void,
  changeAmount: (event: ChangeEvent<HTMLInputElement>) => void,
  changeCategory: (option?: SelectOption) => void,
  changeAccount: (option?: SelectOption) => void,
  changeType: (type? : number) => void,
  toggleIsPaid: () => void,
  submitTransaction: (event: FormEvent<HTMLFormElement>) => Promise<void>,

  isLoading: boolean,
  transaction: Transaction
}

interface ITransactionFormHookProps {
  formData: Transaction,
  onSubmit?: (transaction: Transaction) => void //TODO: UseCallback
}

export function UseTransactionForm(
  {
    formData = defaultTransaction,
    onSubmit
  } : ITransactionFormHookProps
)   : ITransactionFormHook{
  const [transaction, setTransaction] = useState<Transaction>(formData);
  const [isLoading, setLoading] = useState(false);
  const { transactionsApi } = useApisContext();

  const submitTransaction = async function (event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setLoading(true);

    let tra: Transaction;
    
    if (transaction.id) {
      await UseUpdateTransaction(transaction,transactionsApi);
      tra = transaction;
    } else {
      tra = await UseCreateTransaction(transaction,transactionsApi);
    }

    onSubmit?.(tra);
    setTransaction(defaultTransaction); 
    setLoading(false);
  };

  const changeField = function(event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>){
    // if(Object.keys(transaction).includes(event.target.name)){
    setTransaction({
      ...transaction,
      [event.target.name]: event.target.value
    });
    // }
    console.log(transaction);
  };

  const changeAmount = function(event: ChangeEvent<HTMLInputElement>){
    setTransaction({
      ...transaction,
      amount: parseFloat(event.target.value)
    });
  };

  const changeType = function (type? : number) {
    if(type){
      setTransaction({
        ...transaction,
        type
      });
    }
  };

  //TODO: join changeCategory with changeAccount
  const changeCategory = function (option?: SelectOption) {
    if(option?.value){
      setTransaction({
        ...transaction,
        categoryId: option?.value
      });
    }
  };

  const changeAccount = function (option?: SelectOption) {
    if(option?.value){
      setTransaction({
        ...transaction,
        accountId: option?.value
      });
    }
  };

  const toggleIsPaid = function () {
    const commited = transaction.status === TransactionStatus.Committed;
    setTransaction(
      {
        ...transaction,
        status: commited ? TransactionStatus.NotCommitted : TransactionStatus.Committed
      }
    );
  };

  useEffect(
    ()=>{
      setTransaction(formData);
    }, 
    [formData]
  );
  
  return {
    isLoading,
    transaction,
    
    changeField,
    changeType,
    changeAmount,
    changeCategory,
    changeAccount,
    toggleIsPaid,

    submitTransaction,
  };
}