import { render } from '@testing-library/react';

import { CreateTransaction } from '../../../../../__mocks__/types/transaction-builder';

import TransactionListItem from '../../../../../commom/components/transactions/list/item/transaction-list-item';

describe('on render', () =>{
  it('it should show finish date', ()=>{
    const transaction = CreateTransaction();
    const onSelect = jest.fn();
    render(
      <TransactionListItem transaction={transaction} onSelect={onSelect}/>
    );
    expect(true).toBe(false);
  });
  it('it should show transaction type', ()=>{
    const transaction = CreateTransaction();
    const onSelect = jest.fn();
    render(
      <TransactionListItem transaction={transaction} onSelect={onSelect}/>
    );
    expect(true).toBe(false);
  });
  it('it should show the amount of transaction', ()=>{
    const transaction = CreateTransaction();
    const onSelect = jest.fn();
    render(
      <TransactionListItem transaction={transaction} onSelect={onSelect}/>
    );
    expect(true).toBe(false);
  });
  it('it should show category name', ()=>{
    const transaction = CreateTransaction();
    const onSelect = jest.fn();
    render(
      <TransactionListItem transaction={transaction} onSelect={onSelect}/>
    );
    expect(true).toBe(false);
  });
  it('it should show account name', ()=>{
    const transaction = CreateTransaction();
    const onSelect = jest.fn();
    render(
      <TransactionListItem transaction={transaction} onSelect={onSelect}/>
    );
    expect(true).toBe(false);
  });
});

describe('on select', () =>{
  it('should call onSelect method', () =>{
    const transaction = CreateTransaction();
    const onSelect = jest.fn();
    render(
      <TransactionListItem transaction={transaction} onSelect={onSelect}/>
    );
    expect(true).toBe(false);
  });
});
