import { createContext, useContext, useState } from 'react';
import { Account } from '../../../commom/interfaces/account';

type AccountsState = {
  account: Account,
  accounts: Account[]
}

type AccountsContext = [
  AccountsState,
  (state: AccountsState) => void
];

const defaultContextState = {
  account: { name: '', description: '', currency: '', isActive: false } as Account,
  accounts: [] as Account[]
} as AccountsState;

const defaultContext: AccountsContext = [
  defaultContextState,
  // eslint-disable-next-line @typescript-eslint/no-empty-function, @typescript-eslint/no-unused-vars
  (state: AccountsState) => { }
];

const AccountsContext = createContext<AccountsContext>(defaultContext);

//EXPORT
export function AccountsProvider({ children , defaultState}: { children?: JSX.Element | JSX.Element[] , defaultState?: AccountsState }) {
  const [state, setState] = useState({
    accounts: defaultState?.accounts || defaultContextState.accounts, 
    account:  defaultState?.account  || defaultContextState.account
  });

  return (
    <AccountsContext.Provider value={[state, setState]}>
      {children}
    </AccountsContext.Provider>
  );
}

export function useAccountsContext(){
  const context = useContext(AccountsContext);

  if (context === undefined) {
    throw new Error('useAccountsContext must be used within a AccountsProvider');
  }
  return context;
}
