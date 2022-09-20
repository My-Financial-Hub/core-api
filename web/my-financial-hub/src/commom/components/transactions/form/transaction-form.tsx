import { ChangeEvent, FormEvent, useEffect, useState } from 'react';
import { Account } from '../../../interfaces/account';
import { Category } from '../../../interfaces/category';
import { defaultTransaction, Transaction, TransactionStatus } from '../../../interfaces/transaction';
import FormFieldLabel from '../../forms/form-field';
import FormSelect from '../../forms/form-select';
import SelectOption from '../../forms/form-select/types/select-option';

type FormProps = {
  formData?: Transaction,
  accounts: Account[], categories: Category[],
  onSubmit?: (transaction: Transaction) => void
};

export default function TransactionForm(
  {
    formData = defaultTransaction,
    categories, accounts,
    onSubmit
  }: FormProps) {
  const [transaction, setTransaction] = useState<Transaction>(formData);
  const [isLoading, setLoading] = useState(false);

  const submitTransaction = async function (event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setLoading(true);

    let tra: Transaction;

    if (transaction.id) {
      //await useUpdateTransaction(category,categoriesApi);
      tra = transaction;
    } else {
      //tra = await useCreateCategory(transaction,categoriesApi);
      tra = transaction;
    }

    onSubmit?.(tra);
    setTransaction(defaultTransaction);
    setLoading(false);
  };

  useEffect(
    () => {
      setLoading(true);
      setTransaction(formData);
      setLoading(false);
    },
    [formData]
  );

  const selectCategory = function (option?: SelectOption) {
    console.log('yay! it worked again');
  };

  const selectAccount = function (option?: SelectOption) {
    console.log('yay! it worked again');
  };

  const toggleIsPaid = function (event: ChangeEvent<HTMLInputElement>) {
    console.log( event.target.value);
    const commited = transaction.status === TransactionStatus.Committed;
    setTransaction(
      {
        ...transaction,
        status: commited? TransactionStatus.NotCommitted : TransactionStatus.Committed 
      }
    );
  };

  return (
    <form onSubmit={submitTransaction}>
      <div className='row my-2'>
        <FormFieldLabel name='type' title='type'>
          <select>
            {/* TODO : add EnumFormSelect */}
          </select>
        </FormFieldLabel>
      </div>
      <div className='row my-2'>
        <FormFieldLabel name='description' title='description'>
          <textarea name='description'></textarea>
        </FormFieldLabel>
      </div>

      <div className='row my-2'>
        <FormFieldLabel name='category' title='category'>
          <FormSelect
            placeholder='Select a category'
            disabled={isLoading}
            options={
              categories.map(
                cat => (
                  {
                    value: cat.id ?? '',
                    label: cat.name
                  }
                )
              )
            }
            onChangeOption={selectCategory}
          />
        </FormFieldLabel>   
      </div>

      <div className='row my-2'>
        <FormFieldLabel name='account' title='account'>
          <FormSelect
            placeholder='Select an account'
            disabled={isLoading}
            options={
              accounts.map(
                acc => (
                  {
                    value: acc.id ?? '',
                    label: acc.name
                  }
                )
              )
            }
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
            value={transaction.amount}
            disabled={isLoading}
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
            value={transaction.finishDate.toISOString().split('T')[0]}
            onChange={
              (event) => setTransaction({
                ...transaction,
                finishDate: new Date(Date.parse(event.target.value))
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
                  value={transaction.finishDate.toISOString().split('T')[0]}
                  onChange={
                    (event) => setTransaction({
                      ...transaction,
                      finishDate: new Date(Date.parse(event.target.value))
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
            onChange={toggleIsPaid}
          />
        </FormFieldLabel>
      </div>

      <button disabled={isLoading} type='submit'>{!isLoading ? transaction.id ? 'Update' : 'Create' : 'Loading'}</button>
    </form>
  );
}