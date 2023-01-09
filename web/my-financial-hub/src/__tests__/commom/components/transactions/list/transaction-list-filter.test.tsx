import { render } from '@testing-library/react';
import userEvent from '@testing-library/user-event';

import { act } from 'react-dom/test-utils';
import TransactionListFilter from '../../../../../commom/components/transactions/list/filter/transaction-list-filter';
import { TransactionType } from '../../../../../commom/interfaces/transaction';

describe('on start', () => {
  describe('without value',() => {
    it('should set all fields as default value', () => {
      render(
        <TransactionListFilter onFilter={jest.fn()} />
      );
  
      expect(true).toBe(false);
    });
    describe('with value',() => {
      it('should set all fields as the input value', () => {
        render(
          <TransactionListFilter onFilter={jest.fn()} />
        );
    
        expect(true).toBe(false);
      });
    });
  });
});

describe('on submit', () => {
  //TODO: make the test by changing the fields instead of the defaultFilter prop
  it('should send all filtered values on filter', ()=>{
    const filter = {
      types: [
        TransactionType.Earn
      ],
      startDate: new Date(),
      targetDate: new Date(),
      categories: [
        'category'
      ],
      accounts: [
        'account'
      ]
    };
    const onFilter = jest.fn();
    
    const { getByText } = render(
      <TransactionListFilter
        defaultFilter={filter} 
        onFilter={onFilter} 
      />
    );
    
    act(
      ()=>{
        userEvent.click(getByText('Filter'));
      }
    );

    expect(onFilter).toBeCalledWith(filter);
  });
  it('should call "onFilter" method', ()=>{
    const filter = {};
    const onFilter = jest.fn();
    
    const { getByText } = render(
      <TransactionListFilter
        defaultFilter={filter} 
        onFilter={onFilter} 
      />
    );
    
    act(
      ()=>{
        userEvent.click(getByText('Filter'));
      }
    );

    expect(onFilter).toBeCalledTimes(1);
  });
});