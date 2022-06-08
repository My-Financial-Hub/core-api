import { useState } from 'react';

import { Account } from '../../commom/interfaces/account';

import AccountsForm from './components/accounts-form';
import AccountsList from './components/accounts-list';

function AccountsPage() {
  const [account, setAccount] = useState({} as Account);

  return (
    <>
      <div className='container'>
        <h1>Accounts</h1>
        <AccountsForm account={account}/>
        <AccountsList onSelectAccount={(a)=> setAccount(a)} />
      </div>
    </>
  );
}

export default AccountsPage;