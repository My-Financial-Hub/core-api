import * as hooks from '../../commom/hooks/transactions-hooks';
import { Transaction } from '../../commom/interfaces/transaction';
import { getRandomInt } from '../utils/number-utils';

const randTimeOut = getRandomInt(500, 5000);

export function MockUseCreateTransaction(transaction: Transaction, timeout: number = randTimeOut) {

  return jest.spyOn(hooks, 'useCreateTransaction').mockImplementation(
    ()=>{
      jest.setTimeout(timeout);
      return Promise.resolve(transaction);
    }
  );
}

export function MockUseGetTransactions(transaction?: Transaction[], timeout: number = randTimeOut) {
  return jest.spyOn(hooks, 'useGetTransactions').mockImplementation(
    () => {
      return new Promise(
        () => {
          setTimeout(() => {
            if (transaction) {
              return Promise.resolve(transaction);
            } else {
              return Promise.reject([]);
            }
          }, timeout);
        }
      );
    }
  );
}

export function MockUseDeleteTransaction() {
  return jest.spyOn(hooks, 'useDeleteTransaction').mockImplementation(
    async () => {
      setTimeout(() => {
        Promise.resolve();
      }, randTimeOut);
    }
  );
}

export function MockUseUpdateTransaction(account?: Transaction, timeout: number = randTimeOut) {

  return jest.spyOn(hooks, 'useUpdateTransaction').mockImplementation(
    async () => {
      setTimeout(() => {
        Promise.resolve(account);
      }, timeout);
    }
  );
}