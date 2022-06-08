import { useEffect, useState } from 'react';

import AccountApi from '../../../../commom/http/account-api';
import { Account } from '../../../../commom/interfaces/account';

import AccountListItem from './account-list-item';
import Loading from '../../../../commom/components/loading/loading';

interface AccountListProps {
  onSelectAccount: (account: Account) => void
}
const accountsApi = new AccountApi();

function AccountsList({ onSelectAccount }: AccountListProps) {
  const [isLoading, setLoading] = useState(true);
  const [accounts, setAccounts] = useState([] as Account[]);

  const getAccounts = function () {
    setLoading(true);
    accountsApi.GetAllAsync()
      .then(a => {
        setAccounts(a);
        setLoading(false);
      })
      .catch(e => console.error(e));
  };

  const deleteAccount = function (id: string) {
    accountsApi.DeleteAsync(id)
      .then(() => setAccounts(accounts.filter(x => x.id !== id)))
      .catch(e => console.error(e));
  };
  
  useEffect(() => {
    getAccounts();
  }, []);

  return (
    <>
      <div className='container'>
        <div className='row d-flex p-2'>
          <div className='col-3'>
            Name
          </div>
          <div className='col-6'>
            Description
          </div>
          <div className='col-1'>
            Currency
          </div>
          <div className='col-1'>
            Is Active
          </div>
          <div className='col-1'>
            Delete
          </div>
        </div>
        {
          isLoading?
            <Loading />
            :
            accounts.map(
              (account) => (
                <AccountListItem 
                  key={account.id} 
                  account={account} 
                  onRemove={deleteAccount} onSelect={onSelectAccount}
                />
              )
            )
        }
      </div>
    </>
  );
}

export default AccountsList;