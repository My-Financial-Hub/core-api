import { useEffect, useState } from 'react';

import { useAccountsContext } from '../../contexts/accounts-page-context';
import { useApisContext } from '../../../../commom/contexts/api-context';

import Loading from '../../../../commom/components/loading/loading';
import AccountListItem from './account-list-item';

import style from './account-list.module.scss';
import { useGetAccounts } from '../../hooks/accounts-page.hooks';

//import order
/*
1. React libs
2. 3th party libs
3.1 Interfaces
3.2 Global classes
3.3 Contexts
4.1 Global Components
4.2 Components
5. Stylesheets
*/

function AccountsList() {
  const [isLoading, setLoading] = useState(true);
  const [state, setState] = useAccountsContext();
  const { accountsApi } = useApisContext();

  const getAccounts = async function () {
    setLoading(true);

    await useGetAccounts([state, setState], accountsApi);

    setLoading(false);
  };

  useEffect(() => {
    getAccounts();
  }, []);

  return (
    isLoading ? (
      <Loading />
    ) : state.accounts.length > 0 ?
      (
        <div className={`container ${style.table}`}>
          <div className='row d-flex p-2'>
            <div className='d-flex col-11'>
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
            </div>
            <div className='col-1'>
              Delete
            </div>
          </div>
          <div data-testid ='container-content'>
            {
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
        </div>
      ) :
      (
        <div>
          <p>No accounts</p>
          {/* 
          //TODO: error handling
          <p>Not able to show accounts</p>
          <button onClick={getAccounts}>Retry</button> 
          */}
        </div>
      )
  );
}

export default AccountsList;