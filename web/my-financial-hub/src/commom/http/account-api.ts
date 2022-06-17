/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { Account } from '../interfaces/account';
import Api from './api';

export default class AccountApi extends Api<Account>{
  constructor(){
    super(process.env.REACT_APP_API_BASE_URL!, 'accounts');
  }
}