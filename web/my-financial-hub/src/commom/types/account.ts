import { Account } from '../interfaces/account';

export type AccountsState = {
  account: Account,
  accounts: Account[]
}

export type AccountsContext = [
  AccountsState,
  (state: AccountsState) => void
];

export const defaultContextState: AccountsState = {
  account: { name: '', description: '', currency: '', isActive: false } as Account,
  accounts: [] as Account[]
};