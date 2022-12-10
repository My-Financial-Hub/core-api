import { useApisContext } from '../../../contexts/api-context';
import { UseTransactionForm } from './hooks/transaction-form-hook';

import { defaultTransaction, Transaction, TransactionStatus, TransactionType } from '../../../interfaces/transaction';

import FormFieldLabel from '../../forms/form-field';
import EnumFormSelect from '../../forms/form-select/enum-form-select';
import HttpFormSelect from '../../forms/form-select/http-form-select';

type FormProps = {
  formData?: Transaction,
  onSubmit?: (transaction: Transaction) => void
};

export default function TransactionForm(
  {
    formData = defaultTransaction,
    onSubmit
  }: FormProps) {
  const { accountsApi, categoriesApi } = useApisContext();

  const { 
    isLoading ,transaction,
    changeField, toggleIsPaid,
    changeAmount, changeCategory, changeType,
    changeAccount,

    submitTransaction,
  } = UseTransactionForm({formData, onSubmit });

  return (
    <form onSubmit={submitTransaction}>
      <div className='row my-2'>
        <FormFieldLabel name='type' title='type'>
          <EnumFormSelect
            options={TransactionType}
            value={transaction.type}
            onChangeOption={changeType}
            placeholder='Select a type'
            disabled={isLoading}
          />
        </FormFieldLabel>
      </div>
      <div className='row my-2'>
        <FormFieldLabel name='description' title='description'>
          <textarea 
            title='description'
            name='description'
            disabled={isLoading}
            value={transaction.description}
            onChange={changeField}
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
            onChangeOption={changeCategory}
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
            onChangeOption={changeAccount}
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
            onChange={changeAmount}
          />
        </FormFieldLabel>
      </div>

      <div className='row my-2'>
        <FormFieldLabel name='targetdate' title='target date'>
          <input
            title='targetdate'
            name='targetDate'
            type='date'
            disabled={isLoading}
            value={transaction.targetDate.split('T')[0]}
            onChange={changeField}
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
                  name='finishDate'
                  type='date'
                  disabled={isLoading}
                  value={transaction.finishDate.split('T')[0]}
                  onChange={changeField}
                />
              </FormFieldLabel>
            </div>
          ) :
          (
            <>
            </>
          )
      }
      <div className='row my-2'>
        <FormFieldLabel name='ispaid' title='is Paid'>
          <input
            title='ispaid'
            name='isPaid'
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