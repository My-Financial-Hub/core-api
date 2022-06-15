import AccountApi from '../../../commom/http/account-api';
import { Account } from '../../../commom/interfaces/account';

//TODO: find a way to use without params
//TODO : move to commom/types
type AccountsState = {
  account: Account,
  accounts: Account[]
}

type AccountsContext = [
  AccountsState,
  (state: AccountsState) => void
];

const defaultAccount = { 
  name: '', 
  description: '', 
  currency: '', 
  isActive: false 
};

export async function useGetAccounts(context: AccountsContext, accountsApi: AccountApi) {
  const [state, setState] = context;

  const accountsResult = await accountsApi.GetAllAsync();
  setState(
    { 
      ...state, 
      accounts: accountsResult 
    }
  );
}

export async function useCreateAccount(context: AccountsContext, accountsApi: AccountApi) {
  const [state, setState] = context;

  const accountResult = await accountsApi.PostAsync(state.account);
  setState(
    {
      accounts: [...state.accounts, accountResult],
      account: defaultAccount
    }
  );
}

export async function useUpdateAccount(context: AccountsContext, accountsApi: AccountApi) {
  const [state, setState] = context;

  if (state.account.id) {
    const result = await accountsApi.PutAsync(state.account.id, state.account);

    const index = state.accounts.findIndex(obj => obj.id == result.id);
    state.accounts[index] = result;

    setState(
      {
        accounts: state.accounts,
        account: defaultAccount
      }
    );
  }
}

export async function useDeleteAccount(context: AccountsContext, accountsApi: AccountApi, id?: string) {
  if (id) {
    const [state, setState] = context;
    
    await accountsApi.DeleteAsync(id);

    setState(
      {
        accounts: state.accounts.filter(x => x.id !== id),
        account: defaultAccount
      }
    );
  }
}

export async function useSelectAccount(context: AccountsContext, account :Account) {
  const [state, setState] = context;
  setState({ ...state, account: account });
}