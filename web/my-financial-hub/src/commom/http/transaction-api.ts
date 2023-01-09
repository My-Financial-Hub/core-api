/* eslint-disable @typescript-eslint/no-non-null-assertion */
import Api from './api';

import { TransactionFilter } from '../components/transactions/list/types/transaction-filter';

import { createUrlQuery, createUrlQueryNumber, createUrlStartEndDateQuery } from '../utils/query-utils';

import { ServiceResult } from '../interfaces/service-result';
import { Transaction } from '../interfaces/transaction';

import '../utils/global/date-extensions';

export default class TransactionApi extends Api<Transaction>{
  constructor(){
    super(process.env.REACT_APP_API_BASE_URL!, 'transactions');
  }
}

const _baseUrl = `${process.env.REACT_APP_API_BASE_URL}/transactions`;

//TODO: move to TransactionFilter
function getFilterQuery(filter?: TransactionFilter): string{
  let query = '';
  if(filter){
    query += createUrlQueryNumber('types',filter.types);
    query += createUrlStartEndDateQuery(filter.startDate,filter.targetDate);
    query += createUrlQuery('accounts',filter.accounts);
    query += createUrlQuery('categories',filter.categories);

    if(query.length >1){
      query = query.substring(0,query.length - 1);
    }
  }
  return query;
}

export async function FetchTransactions(filter?: TransactionFilter): Promise<ServiceResult<Transaction[]>>{
  const query = getFilterQuery(filter);
  const result = await fetch(_baseUrl + '?' + query);
  const json = await result.json() as ServiceResult<Transaction[]>;

  if (!result.ok) {
    throw json;//TODO:
  }

  return json;
}