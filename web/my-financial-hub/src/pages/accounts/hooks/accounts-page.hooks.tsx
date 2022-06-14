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

export async function useGetAccounts(context: AccountsContext, accountsApi: AccountApi) {
  const [state, setState] = context;

  const accountsResult = await accountsApi.GetAllAsync();
  setState({ ...state, accounts: accountsResult });
}

export async function useCreateAccount(context: AccountsContext, accountsApi: AccountApi) {
  const [state, setState] = context;

  const accountResult = await accountsApi.PostAsync(state.account);
  setState(
    {
      accounts: [...state.accounts, accountResult],
      account: { name: '', description: '', currency: '', isActive: false } as Account
    }
  );
}

export async function useUpdateAccount(context: AccountsContext, accountsApi: AccountApi) {
  const [state, setState] = context;

  console.log(state.account);
  if (state.account.id) {
    const result = await accountsApi.PutAsync(state.account.id, state.account);

    const index = state.accounts.findIndex(obj => obj.id == result.id);
    state.accounts[index] = result;

    setState(
      {
        accounts: state.accounts,
        account: { name: '', description: '', currency: '', isActive: false } as Account
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
        account: { name: '', description: '', currency: '', isActive: false } as Account,
        accounts: state.accounts.filter(x => x.id !== id)
      }
    );
  }
}


export async function useSelectAccount(context: AccountsContext, account :Account) {
  const [state, setState] = context;
  setState({ ...state, account: account });
}