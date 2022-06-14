import AccountApi from '../../../commom/http/account-api';
import { Account } from '../../../commom/interfaces/account';

type AccountsState = {
  account: Account,
  accounts: Account[]
}

type AccountsContext = [
  AccountsState,
  (state: AccountsState) => void
];

/**
 * @deprecated Use Hooks
**/
export default class AccountsService {
  private readonly _accountsApi: AccountApi;

  constructor(accountsApi: AccountApi) {
    this._accountsApi = accountsApi;
  }

  async createAccount(context: AccountsContext) {
    const [state, setState] = context;
  
    const accountResult = await this._accountsApi.PostAsync(state.account);
    setState(
      {
        accounts: [...state.accounts, accountResult],
        account: { name: '', description: '', currency: '', isActive: false } as Account
      }
    );
  }

  async updateAccount(context: AccountsContext){
    const [state, setState] = context;

    if (state.account.id) {
      const result = await this._accountsApi.PutAsync(state.account.id, state.account);
      
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
}