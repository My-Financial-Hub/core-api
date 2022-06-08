import { useState, FormEvent } from 'react';
import Loading from '../../../../commom/components/loading/loading';

import AccountApi from '../../../../commom/http/account-api';
import { Account } from '../../../../commom/interfaces/account';
import { useAccountsContext } from '../../contexts/accounts-page-context';

const accountsApi = new AccountApi();

function AccountsForm() {
  const [isLoading, setLoading] = useState(false);
  const [state, setState] = useAccountsContext();

  const submitAccount = function (event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setLoading(true);
    accountsApi.PostAsync(state.account)
      .then((accountResult) => {
        setState(
          {
            accounts: [...state.accounts, accountResult],
            account: { name: '', description: '', currency: '', isActive: false } as Account
          }
        );
        setLoading(false);
      });
  };

  const changeAccount = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newAccount = {
      ...state.account,
      [event.target.name]: event.target.value
    } as Account;
    setState({ ...state, account: newAccount });
  };

  const changeAccountActive = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newAccount = {
      ...state.account,
      isActive: event.target.value == 'on'
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
            <label htmlFor='name'>Name</label>
            <input name='name' title='name'
              type='text'
              placeholder='Insert Account name'
              maxLength={50}
              required
              value={state.account.name}
              onChange={changeAccount}
            />

            <label htmlFor='description'>Description</label>
            <input name='description' title='description'
              type='text'
              placeholder='Insert Account description'
              maxLength={500}
              value={state.account.description}
              onChange={changeAccount}
            />

            <label htmlFor='currency'>Currency</label>
            <input name='currency' title='currency'
              type='text'
              placeholder='Insert Account currency'
              maxLength={50}
              value={state.account.currency}
              onChange={changeAccount}
            />

            <label htmlFor='isActive'>Is Active</label>
            <input
              name='isActive' title='isActive'
              type='checkbox'
              checked={state.account.isActive}
              onChange={changeAccountActive}
            />

            <input type='submit' />
          </form>
      }
    </>
  );
}

export default AccountsForm;
