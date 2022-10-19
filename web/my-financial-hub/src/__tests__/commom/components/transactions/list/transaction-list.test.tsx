import { render } from '@testing-library/react';

import { CreateTransaction } from '../../../../../__mocks__/types/transaction-builder';

import TransactionList from '../../../../../commom/components/transactions/list/transaction-list';

describe('on render', () =>{
  describe('with data', () =>{
    it('should show all items',() =>{
      render(
        <TransactionList />
      );
      expect(true).toBe(false);
    });
    it('should make a get request',() =>{
      render(
        <TransactionList />
      );
      expect(true).toBe(false);
    });
  });
  describe('without data', () =>{
    it('should show empty message',() =>{
      render(
        <TransactionList />
      );
      expect(true).toBe(false);
    });
  });
});

describe('on loading',()=>{
  it('should render loading component', () =>{
    render(
      <TransactionList />
    );
    expect(true).toBe(false);
  });
});

describe('on select',()=>{
  it('should call OnSelect method', () =>{
    render(
      <TransactionList />
    );
    expect(true).toBe(false);
  });
  it('should call OnSelect method with the selected transaction', () =>{
    render(
      <TransactionList />
    );
    expect(true).toBe(false);
  });
});