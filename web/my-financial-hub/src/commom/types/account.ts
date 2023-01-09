import { Account } from '../interfaces/account';

export type AccountsState = {
  hasError: boolean,
  error: {
    message: string
  },
  account: Account,
  accounts: Account[]
}

export type AccountsContext = [
  AccountsState,
  (state: AccountsState) => void
];

export const defaultContextState: AccountsState = {
  hasError: false,
  error: {
    message: ''
  },
  account: { name: '', description: '', currency: '', isActive: false } as Account,
  accounts: [] as Account[]
};