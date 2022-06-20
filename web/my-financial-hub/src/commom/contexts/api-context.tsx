import { createContext, useContext } from 'react';
import AccountApi from '../http/account-api';
type ApisContext = {
  accountsApi: AccountApi
};

const defaultContext = {
  accountsApi: new AccountApi()
} as ApisContext;

const ApisContext = createContext<ApisContext>(defaultContext);

//EXPORT
export function ApisContextProvider({ children }: { children?: JSX.Element | JSX.Element[] }) {
  return (
    <ApisContext.Provider value={defaultContext}>
      {children}
    </ApisContext.Provider>
  );
}

export function useApisContext(){
  const context = useContext(ApisContext);

  if (context === undefined) {
    throw new Error('useApisContext must be used within a ApisContextProvider');
  }
  return context;
}
