import { useEffect, useState} from 'react';

import AccountApi from '../../../../commom/http/account-api';

import AccountListItem from './account-list-item';
import Loading from '../../../../commom/components/loading/loading';
import { useAccountsContext } from '../../contexts/accounts-page-context';

const accountsApi = new AccountApi();//TODO: move to context

function AccountsList() {
  const [isLoading, setLoading] = useState(true);
  const [state, setState] = useAccountsContext();

  const getAccounts = function () {
    setLoading(true);
    accountsApi.GetAllAsync()
      .then(accountsResult => {
        setState({...state, accounts: accountsResult});
        setLoading(false);
      })
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
            state.accounts.map(
              (account) => (
                <AccountListItem 
                  key={account.id} 
                  account={account} 
                />
              )
            )
        }
      </div>
    </>
  );
}

export default AccountsList;