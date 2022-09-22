/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { Transaction } from '../interfaces/transaction';
import Api from './api';

export default class TransactionApi extends Api<Transaction>{
  constructor(){
    super(process.env.REACT_APP_API_BASE_URL!, 'transactions');
  }
}