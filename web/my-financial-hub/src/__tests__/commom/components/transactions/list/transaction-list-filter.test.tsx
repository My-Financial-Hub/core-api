import { render, screen } from '@testing-library/react';

import { act } from 'react-dom/test-utils';
import TransactionListFilter from '../../../../../commom/components/transactions/list/filter/transaction-list-filter';

describe('on start', () => {
  describe('without value',() => {
    it('should set all fields as defaul value', () => {
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
  it('should send all filtered values', ()=>{
    const onFilter = jest.fn();
    render(
      <TransactionListFilter onFilter={onFilter} />
    );
    expect(true).toBe(false);
  });
  it('should call "onFilter" method', ()=>{
    const onFilter = jest.fn();
    render(
      <TransactionListFilter onFilter={onFilter} />
    );
    expect(true).toBe(false);
  });
});