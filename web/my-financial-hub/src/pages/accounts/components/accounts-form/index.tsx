import { useState, FormEvent, useRef } from 'react';

import { Account } from '../../../../commom/interfaces/account';

import { useApisContext } from '../../../../commom/contexts/api-context';
import { useAccountsContext } from '../../contexts/accounts-page-context';

import FormFieldLabel from '../../../../commom/components/forms/form-field';
import Loading from '../../../../commom/components/loading/loading';

import style from './accounts-form.module.scss';
import { useCreateAccount, useUpdateAccount } from '../../hooks/accounts-page.hooks';

function AccountsForm() {
  const [isLoading, setLoading] = useState(false);

  const [state, setState] = useAccountsContext();
  const { accountsApi } = useApisContext();

  const nameInputRef = useRef<HTMLInputElement>(null);

  const submitAccount = async function (event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setLoading(true);

    if (state.account.id) {
      await useUpdateAccount([state, setState], accountsApi);
    } else {
      await useCreateAccount([state, setState], accountsApi);
    }

    setLoading(false);
    setTimeout(()=> nameInputRef.current?.focus() , 100);
  };

  const changeAccountField = function(event: React.ChangeEvent<HTMLInputElement>){
    const newAccount = {
      ...state.account,
      [event.target.name]: event.target.value
    } as Account;
    setState({ ...state, account: newAccount });
  };

  const changeAccountActiveField = function(){
    const newAccount = {
      ...state.account,
      isActive: !state.account.isActive
    } as Account;
    setState({ ...state, account: newAccount });
  };

  return (
    <>
      {
        isLoading ?
          <Loading />
          :
          <form onSubmit={submitAccount} className="container mb-4">
            <div className='row align-items-xl-end my-2'>
              <div className='col-5'>
                <FormFieldLabel name='name' title='Name'>
                  <input name='name' title='name'
                    ref={nameInputRef}
                    type='text'
                    placeholder='Insert Account name'
                    maxLength={50}
                    required
                    value={state.account.name}
                    onChange={changeAccountField}
                  />
                </FormFieldLabel>
              </div>

              <div className='col-2'>
                <FormFieldLabel name='currency' title='Currency'>
                  <input name='currency' title='currency'
                    type='text'
                    placeholder='Insert Account currency'
                    maxLength={50}
                    value={state.account.currency}
                    onChange={changeAccountField}
                  />
                </FormFieldLabel>
              </div>

              <div className='col-2'>
                <FormFieldLabel name='isActive' title='Is Active' direction='row'>
                  <input
                    className={style.checkbox}
                    name='isActive' title='isActive'
                    type='checkbox'
                    checked={state.account.isActive ?? false}
                    onChange={changeAccountActiveField}
                  />
                </FormFieldLabel>
              </div>
            </div>

            <div className='row align-items-xl-end my-2'>
              <div className='col-5'>
                <FormFieldLabel name='description' title='Description'>
                  <input name='description' title='description'
                    type='text'
                    placeholder='Insert Account description'
                    maxLength={500}
                    value={state.account.description}
                    onChange={changeAccountField}
                  />
                </FormFieldLabel>
              </div>
              <div className='col-1'>
                <button type='submit'>Submit</button>
              </div>
            </div>
          </form>
      }
    </>
  );
}

export default AccountsForm;
