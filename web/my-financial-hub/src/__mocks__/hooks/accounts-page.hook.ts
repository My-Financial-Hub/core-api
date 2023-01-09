import * as hooks from '../../pages/accounts/hooks/accounts-page.hooks';
import { Account, defaultAccount } from '../../commom/interfaces/account';
import { getRandomInt } from '../utils/number-utils';

const randTimeOut = getRandomInt(500,5000);

export function mockUseGetAccounts(accounts?: Account[], timeout: number = randTimeOut) {
  return jest.spyOn(hooks, 'useGetAccounts').mockImplementation(
    async (context) => {
      setTimeout(() => {
        const [state, setState] = context;
        if (accounts) {
          setState({
            ...state,
            accounts
          });
          Promise.resolve();
        } else {
          Promise.reject('no accounts');
        }
      }, timeout);
    }
  );
}

export function mockUseDeleteAccount(){
  return jest.spyOn(hooks, 'useDeleteAccount').mockImplementation(
    async (context) => {
      setTimeout(() => {
        const [state, setState] = context;
        setState(
          {
            ...state,
            account: defaultAccount
          }
        );

        Promise.resolve();
      }, randTimeOut);
    }
  );
}

export function mockUseUpdateAccount(account?: Account){
  const randTimeOut = getRandomInt(500,5000);

  return jest.spyOn(hooks, 'useUpdateAccount').mockImplementation(
    async (context) => {
      setTimeout(() => {
        const [state, setState] = context;
        setState(
          {
            ...state,
            account: account ?? defaultAccount
          }
        );

        Promise.resolve();
      }, randTimeOut);
    }
  );
}

export function mockUseCreateAccount(account?: Account){
  const randTimeOut = getRandomInt(500,5000);

  return jest.spyOn(hooks, 'useCreateAccount').mockImplementation(
    async (context) => {
      setTimeout(() => {
        const [state, setState] = context;
        setState(
          {
            ...state,
            account: account ?? defaultAccount
          }
        );

        Promise.resolve();
      }, randTimeOut);
    }
  );
}