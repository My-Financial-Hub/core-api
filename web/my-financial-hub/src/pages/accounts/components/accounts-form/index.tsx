import { useState, FormEvent } from 'react';
import FormFieldLabel from '../../../../commom/components/forms/form-field';

import Loading from '../../../../commom/components/loading/loading';

import AccountApi from '../../../../commom/http/account-api';
import { Account } from '../../../../commom/interfaces/account';
import { useAccountsContext } from '../../contexts/accounts-page-context';

import style from './accounts-form.module.scss';

const accountsApi = new AccountApi();

function AccountsForm() {
  const [isLoading, setLoading] = useState(false);
  const [state, setState] = useAccountsContext();

  const updateAccount = async function () {
    if (state.account.id) {
      accountsApi.PutAsync(state.account.id, state.account)
        .then(() => {
          const index = state.accounts.findIndex(obj => obj.id == state.account.id);
          state.accounts[index] = state.account;
          resetForm();
          setLoading(false);
        });
    }
  };

  const createAccount = async function () {
    accountsApi.PostAsync(state.account)
      .then((accountResult) => {
        setState(
          {
            accounts: [...state.accounts, accountResult],
            account: { name: '', description: '', currency: '', isActive: false } as Account
          }
        );
      });
  };

  const submitAccount = function (event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setLoading(true);

    if (state.account.id) {
      updateAccount().then(() => setLoading(false));
    } else {
      createAccount().then(() => setLoading(false));
    }
  };

  const resetForm = function () {
    setState({ ...state, account: { name: '', description: '', currency: '', isActive: false } as Account });
  };

  const changeAccountField = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newAccount = {
      ...state.account,
      [event.target.name]: event.target.value
    } as Account;
    setState({ ...state, account: newAccount });
  };

  const changeAccountActiveField = () => {
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
          <form onSubmit={(e) => submitAccount(e)} className="container mb-4">
            <div className='row align-items-xl-end my-2'>
              <div className='col-5'>
                <FormFieldLabel name='name' title='Name'>
                  <input name='name' title='name'
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
                    checked={state.account.isActive}
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
                <input type='submit' />
              </div>
            </div>
          </form>
      }
    </>
  );
}

export default AccountsForm;
