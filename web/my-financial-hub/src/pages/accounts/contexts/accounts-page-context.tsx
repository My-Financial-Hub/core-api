import { createContext, useContext, useState } from 'react';
import { AccountsState, AccountsContext, defaultContextState } from '../../../commom/types/account';

const defaultContext: AccountsContext = [
  defaultContextState,
  // eslint-disable-next-line @typescript-eslint/no-empty-function, @typescript-eslint/no-unused-vars
  (state: AccountsState) => { }
];

const AccountContext = createContext<AccountsContext>(defaultContext);

//EXPORT
export function AccountsProvider({ children , defaultState}: { children?: JSX.Element | JSX.Element[] , defaultState?: AccountsState }) {
  const [state, setState] = useState({
    accounts: defaultState?.accounts || defaultContextState.accounts, 
    account:  defaultState?.account  || defaultContextState.account
  });

  return (
    <AccountContext.Provider value={[state, setState]}>
      {children}
    </AccountContext.Provider>
  );
}

export function useAccountsContext(){
  const context = useContext(AccountContext);

  if (context === undefined) {
    throw new Error('useAccountsContext must be used within a AccountsProvider');
  }
  return context;
}
