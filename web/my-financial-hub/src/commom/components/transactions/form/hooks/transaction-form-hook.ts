import { ChangeEvent, FormEvent, useEffect, useState } from 'react';

import { useApisContext } from '../../../../contexts/api-context';
import { UseCreateTransaction, UseUpdateTransaction } from '../../../../hooks/transactions-hooks';
import UseTransactionValidator from '../../../../hooks/validators/transaction-validator-hooks';

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
  getErrorMessage: (field: string) => string | undefined,

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
  const [ transaction, setTransaction ] = useState<Transaction>(formData);
  const [ isLoading, setLoading ] = useState(false);
  const { transactionsApi } = useApisContext();
  const { hasError ,getErrorMessage ,validate } = UseTransactionValidator();

  const submitTransaction = async function (event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setLoading(true);

    validate(transaction);
    if(!hasError){
      let tra: Transaction;
      
      if (transaction.id) {
        await UseUpdateTransaction(transaction,transactionsApi);
        tra = transaction;
      } else {
        tra = await UseCreateTransaction(transaction,transactionsApi);
      }
  
      onSubmit?.(tra);
      setTransaction(defaultTransaction); 
    }

    setLoading(false);
  };

  const changeField = function(event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) : void{
    const { name, value } = event.target;
    setTransaction({
      ...transaction,
      [name]: value
    });
  };

  const changeAmount = function(event: ChangeEvent<HTMLInputElement>) : void{
    setTransaction({
      ...transaction,
      amount: parseFloat(event.target.value)
    });
  };

  const changeType = function (type? : number) : void{
    if(type){
      setTransaction({
        ...transaction,
        type
      });
    }
  };

  //TODO: join changeCategory with changeAccount
  const changeCategory = function (option?: SelectOption) : void{
    if(option?.value){
      setTransaction({
        ...transaction,
        categoryId: option?.value
      });
    }
  };

  const changeAccount = function (option?: SelectOption) : void{
    if(option?.value){
      setTransaction({
        ...transaction,
        accountId: option?.value
      });
    }
  };

  const toggleIsPaid = function () : void{
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
    getErrorMessage,

    changeField,
    changeType,
    changeAmount,
    changeCategory,
    changeAccount,
    toggleIsPaid,

    submitTransaction,
  };
}