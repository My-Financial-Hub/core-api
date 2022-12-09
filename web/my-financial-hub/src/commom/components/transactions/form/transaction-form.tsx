import { FormEvent, useEffect, useState } from 'react';
import { useApisContext } from '../../../contexts/api-context';
import { UseCreateTransaction, UseUpdateTransaction } from '../../../hooks/transactions-hooks';
import { defaultTransaction, Transaction, TransactionStatus, TransactionType } from '../../../interfaces/transaction';
import FormFieldLabel from '../../forms/form-field';
import EnumFormSelect from '../../forms/form-select/enum-form-select';
import HttpFormSelect from '../../forms/form-select/http-form-select';
import SelectOption from '../../forms/form-select/types/select-option';

type FormProps = {
  formData?: Transaction,
  onSubmit?: (transaction: Transaction) => void
};

export default function TransactionForm(
  {
    formData = defaultTransaction,
    onSubmit
  }: FormProps) {
  const [transaction, setTransaction] = useState<Transaction>(formData);
  const [isLoading, setLoading] = useState(false);
  const { transactionsApi, accountsApi, categoriesApi } = useApisContext();

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

  const selectCategory = function (option?: SelectOption) {
    if(option?.value){
      setTransaction({
        ...transaction,
        categoryId: option?.value
      });
    }
  };

  const selectAccount = function (option?: SelectOption) {
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

  return (
    <form onSubmit={submitTransaction}>
      <div className='row my-2'>
        <FormFieldLabel name='type' title='type'>
          <EnumFormSelect
            options={TransactionType}
            value={transaction.type}
            onChangeOption={(type)=>{
              if(type){
                setTransaction({
                  ...transaction,
                  type
                });
              }
            }}
            placeholder='Select a type'
            disabled={isLoading}
          />
        </FormFieldLabel>
      </div>
      <div className='row my-2'>
        <FormFieldLabel name='description' title='description'>
          <textarea 
            title='description'
            disabled={isLoading}
            value={transaction.description}
            onChange={
              (event) => setTransaction({
                ...transaction,
                description: event.target.value
              })
            }
          >
          </textarea>
        </FormFieldLabel>
      </div>

      <div className='row my-2'>
        <FormFieldLabel name='category' title='category'>
          <HttpFormSelect 
            api={categoriesApi}
            placeholder='Select a category'
            disabled={isLoading}
            value={transaction.categoryId}
            onChangeOption={selectCategory}
          />
        </FormFieldLabel>
      </div>

      <div className='row my-2'>
        <FormFieldLabel name='account' title='account'>
          <HttpFormSelect 
            api={accountsApi}
            placeholder='Select an account'
            disabled={isLoading}
            value={transaction.accountId}
            onChangeOption={selectAccount}
          />
        </FormFieldLabel>
      </div>

      <div className='row my-2'>
        <FormFieldLabel name='amount' title='amount'>
          <input
            title='amount'
            type='number'
            step="0.01"
            min="0.01"
            placeholder='Insert Transaction Amount'
            disabled={isLoading}
            value={transaction.amount}
            onChange={
              (event) => setTransaction({
                ...transaction,
                amount: parseFloat(event.target.value)
              })
            }
          />
        </FormFieldLabel>
      </div>

      <div className='row my-2'>
        <FormFieldLabel name='targetdate' title='target date'>
          <input
            title='targetdate'
            type='date'
            disabled={isLoading}
            value={transaction.targetDate.split('T')[0]}
            onChange={
              (event) => setTransaction({
                ...transaction,
                targetDate: event.target.value
              })
            }
          />
        </FormFieldLabel>
      </div>

      {
        transaction.status === TransactionStatus.Committed ?
          (
            <div className='row my-2'>
              <FormFieldLabel name='finishdate' title='finish date'>
                <input
                  title='finishdate'
                  type='date'
                  disabled={isLoading}
                  value={transaction.finishDate.split('T')[0]}
                  onChange={
                    (event) => setTransaction({
                      ...transaction,
                      finishDate: event.target.value
                    })
                  }
                />
              </FormFieldLabel>
            </div>
          ) :
          (
            <div>

            </div>
          )
      }
      <div className='row my-2'>
        <FormFieldLabel name='ispaid' title='is Paid'>
          <input
            title='ispaid'
            type="checkbox"
            disabled={isLoading}
            checked={transaction.status === TransactionStatus.Committed}
            onChange={toggleIsPaid}
          />
        </FormFieldLabel>
      </div>

      <button disabled={isLoading} type='submit'>{!isLoading ? transaction.id ? 'Update' : 'Create' : 'Loading'}</button>
    </form>
  );
}