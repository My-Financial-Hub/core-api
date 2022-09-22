import { createContext, useContext } from 'react';

import AccountApi from '../http/account-api';
import CategoryApi from '../http/category-api';
import TransactionApi from '../http/transaction-api';

type ApisContext = {
  accountsApi: AccountApi,
  categoriesApi: CategoryApi,
  transactionsApi: TransactionApi,
};

const defaultContext = {
  accountsApi:    new AccountApi(),
  categoriesApi:  new CategoryApi(),
  transactionsApi:  new TransactionApi()
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
