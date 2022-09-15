import { FormEvent, useEffect, useState } from 'react';
import { Account } from '../../../interfaces/account';
import { Category } from '../../../interfaces/category';
import { defaultTransaction, Transaction } from '../../../interfaces/transaction';
import FormFieldLabel from '../../forms/form-field';
import FormSelect from '../../forms/form-select';
import SelectOption from '../../forms/form-select/types/select-option';

type FormProps = {
  formData?: Transaction,
  accounts: Account[],categories: Category[],
  onSubmit?: (transaction: Transaction) => void
};

export default function TransactionForm({ formData = defaultTransaction,categories, onSubmit }: FormProps) {
  const [transaction, setTransaction] = useState<Transaction>(formData);
  const [isLoading, setLoading] = useState(false);

  const submitTransaction = async function (event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setLoading(true);

    let tra : Transaction;

    if(transaction.id){
      //await useUpdateTransaction(category,categoriesApi);
      tra = transaction;
    }else{
      //tra = await useCreateCategory(transaction,categoriesApi);
      tra = transaction;
    }

    onSubmit?.(tra);
    setTransaction(defaultTransaction);
    setLoading(false);
  };

  useEffect(
    () =>{
      setTransaction(formData);
    },
    [formData]
  );

  const selectCategory = function (option?: SelectOption) {
    console.log('yay! it worked again');
  };

  return (
    <form>
      <div className='row my-2'>
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


      <button disabled={isLoading} type='submit'>{!isLoading? transaction.id? 'Update' : 'Create' : 'Loading'}</button>
    </form>
  );
}