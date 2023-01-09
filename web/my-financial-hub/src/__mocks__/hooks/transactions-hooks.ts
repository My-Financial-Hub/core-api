import * as hooks from '../../commom/hooks/transactions-hooks';
import { Transaction } from '../../commom/interfaces/transaction';
import { getRandomInt } from '../utils/number-utils';

const randTimeOut = getRandomInt(10, 100);

export function MockUseCreateTransaction(transaction: Transaction, timeout: number = randTimeOut) {

  return jest.spyOn(hooks, 'UseCreateTransaction').mockImplementation(
    () => {
      jest.setTimeout(timeout);
      return Promise.resolve(transaction);
    }
  );
}

export function MockUseGetTransactions(transactions?: Transaction[], timeout: number = randTimeOut) {
  return jest.spyOn(hooks, 'UseGetTransactions').mockImplementation(
    async () => {
      jest.setTimeout(timeout);
      if (transactions) {
        return Promise.resolve(transactions);
      } else {
        return Promise.reject([]);
      }
    }
  );
}

export function MockUseDeleteTransaction() {
  return jest.spyOn(hooks, 'UseDeleteTransaction').mockImplementation(
    async () => {
      setTimeout(() => {
        Promise.resolve();
      }, randTimeOut);
    }
  );
}

export function MockUseUpdateTransaction(account?: Transaction, timeout: number = randTimeOut) {

  return jest.spyOn(hooks, 'UseUpdateTransaction').mockImplementation(
    async () => {
      setTimeout(() => {
        Promise.resolve(account);
      }, timeout);
    }
  );
}