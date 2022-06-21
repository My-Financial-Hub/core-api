import AccountApi from '../../../commom/http/account-api';
import { Account , defaultAccount } from '../../../commom/interfaces/account';
import { AccountsContext } from '../../../commom/types/account';

//TODO: find a way to use without params
export async function useGetAccounts(context: AccountsContext, accountsApi: AccountApi) {
  const [state, setState] = context;
  try {
    const accountsResult = await accountsApi.GetAllAsync();
    setState(
      {
        ...state,
        hasError: accountsResult.hasError,
        error:    accountsResult.error,
        accounts: accountsResult.data
      }
    );
  } catch (error) {
    console.error(error);
  }
}

export async function useCreateAccount(context: AccountsContext, accountsApi: AccountApi) {
  const [state, setState] = context;

  const accountResult = await accountsApi.PostAsync(state.account);
  setState(
    {
      hasError: accountResult.hasError,
      error: accountResult.error,
      accounts: [...state.accounts, accountResult.data],
      account: defaultAccount
    }
  );
}

export async function useUpdateAccount(context: AccountsContext, accountsApi: AccountApi) {
  const [state, setState] = context;

  if (state.account.id) {
    const result = await accountsApi.PutAsync(state.account.id, state.account);

    if(result.hasError){ 
      setState(
        {
          ...state,
          hasError: true,
          error: result.error
        }
      );
    }else{
      const index = state.accounts.findIndex(obj => obj.id == result.data.id);
      state.accounts[index] = result.data;
  
      setState(
        {
          hasError: false,
          error: { message: '' },
          accounts: state.accounts,
          account: defaultAccount
        }
      );
    }
  }
}

export async function useDeleteAccount(context: AccountsContext, accountsApi: AccountApi, id?: string) {
  if (id) {
    const [state, setState] = context;
    await accountsApi.DeleteAsync(id);

    setState(
      {
        hasError: false,
        error: { message: '' },
        accounts: state.accounts.filter(x => x.id !== id),
        account: defaultAccount
      }
    );
  }
}

export async function useSelectAccount(context: AccountsContext, account: Account) {
  const [state, setState] = context;
  setState({ ...state, account: account });
}