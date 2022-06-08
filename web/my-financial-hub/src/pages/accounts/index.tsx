import { AccountsProvider } from './contexts/accounts-page-context';
import AccountsForm from './components/accounts-form';
import AccountsList from './components/accounts-list';


function AccountsPage() {
  return (
    <AccountsProvider>
      <div className='container'>
        <h1>Accounts</h1>
        <AccountsForm />
        <AccountsList/>
      </div>
    </AccountsProvider>
  );
}

export default AccountsPage;