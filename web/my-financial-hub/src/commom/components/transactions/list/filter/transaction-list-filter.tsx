import { useState } from 'react';

import { TransactionFilter } from '../types/transaction-filter';

import HttpFormSelect from '../../../forms/form-select/http-form-select';

import { useApisContext } from '../../../../contexts/api-context';
import EnumFormSelect from '../../../forms/form-select/enum-form-select';
import { TransactionType } from '../../../../interfaces/transaction';

interface ITransactionListFilterProps{
  filter?: TransactionFilter,
  onFilter: (filter: TransactionFilter) => void
}

export default function TransactionListFilter({ onFilter }: ITransactionListFilterProps) {
  const [isLoading, setLoading] = useState(false);
  const [filter,setFilters] = useState<TransactionFilter>({} as TransactionFilter);

  const { accountsApi, categoriesApi } = useApisContext();

  return (
    <div>
      <h3>Filters</h3>
      <form onSubmit={()=> onFilter(filter)}>
        <div>
          <label>Accounts</label>
          <HttpFormSelect
            api={accountsApi}
            placeholder='Select an account'
            disabled={isLoading}
            onChangeOption={
              (selectedOption) => {
                if(selectedOption?.value){
                  setFilters({
                    ...filter,
                    accounts:[
                      selectedOption?.value
                    ]
                  });
                }
              }
            }
          />
        </div>

        <label>Categories</label>
        <HttpFormSelect
          api={categoriesApi}
          placeholder='Select an category'
          disabled={isLoading}
          onChangeOption={
            (selectedOption) => {
              if(selectedOption?.value){
                setFilters({
                  ...filter,
                  categories:[
                    selectedOption?.value
                  ]
                });
              }
            }
          }
        />

        <label>Type</label>
        <EnumFormSelect
          options={TransactionType}
          placeholder='Select a type'
          disabled={true}
        />

        <div>
          <label>Start Date</label>
          <input
            title='startDate'
            type='date'
            disabled={isLoading}
            onChange={
              (e)=>{
                setFilters({
                  ...filter,
                  startDate: new Date(Date.parse(e.target.value))
                });
              }
            }
          />
        </div>

        <div>
          <label>Target Date</label>
          <input
            title='targetdate'
            type='date'
            disabled={isLoading}
            onChange={
              (e)=>{
                setFilters({
                  ...filter,
                  targetDate: new Date(Date.parse(e.target.value))
                });
              }
            }
          />
        </div>

        <div>
          <label>Min. amount</label>
          <input
            title='minamount'
            type='number'
            step="0.01"
            min="0.01"
            placeholder='Insert Min. Transaction Amount'
            disabled={isLoading}
            onChange={
              (e)=>{
                setFilters({
                  ...filter,
                  minAmount: parseFloat(e.target.value)
                });
              }
            }
          />
        </div>

        <div>
          <label>Max. amount</label>
          <input
            title='maxamount'
            type='number'
            step="0.01"
            min="0.01"
            placeholder='Insert Max. Transaction Amount'
            disabled={isLoading}
            onChange={
              (e)=>{
                setFilters({
                  ...filter,
                  maxAmount: parseFloat(e.target.value)
                });
              }
            }
          />
        </div>

        <input type='submit' />
      </form>
    </div>
  );
}