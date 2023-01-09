import { act, render } from '@testing-library/react';
import userEvent from '@testing-library/user-event';

import { CreateTransaction } from '../../../../../__mocks__/types/transaction-builder';

import TransactionListItem from '../../../../../commom/components/transactions/list/item/transaction-list-item';
import { TransactionType } from '../../../../../commom/interfaces/transaction';
import { enumToString } from '../../../../../commom/utils/enum-utils';

describe('on render', () =>{
  it('it should show finish date', ()=>{
    const transaction = CreateTransaction();
    const onSelect = jest.fn();

    const { getByText } = render(
      <TransactionListItem 
        transaction={transaction} 
        onSelect={onSelect}
      />
    );

    const formatedDate = transaction.finishDate;
    const dateField = getByText(formatedDate,{ exact: false });
    expect(dateField).toBeInTheDocument();
  });
  it('it should show transaction type', ()=>{
    const transaction = CreateTransaction();
    const onSelect = jest.fn();
    
    const { getByText } = render(
      <TransactionListItem 
        transaction={transaction} 
        onSelect={onSelect}
      />
    );

    const transactionType = enumToString(TransactionType, transaction.type);
    const typeField = getByText(transactionType,{ exact: false });
    expect(typeField).toBeInTheDocument();
  });
  it('it should show the amount of transaction', ()=>{
    const transaction = CreateTransaction();
    const onSelect = jest.fn();

    const { getByText } = render(
      <TransactionListItem transaction={transaction} onSelect={onSelect}/>
    );

    const transactionAmount = transaction.amount;
    const  amountField = getByText(transactionAmount,{ exact: false });
    expect(amountField).toBeInTheDocument();
  });
  it('it should show account name', ()=>{
    const transaction = CreateTransaction();
    const onSelect = jest.fn();
    
    const { getByText } = render(
      <TransactionListItem transaction={transaction} onSelect={onSelect}/>
    );

    const transactionAccountName = transaction.account?.name ?? '';
    const  accountField = getByText(transactionAccountName,{ exact: false });
    expect(accountField).toBeInTheDocument();
  });
  it('it should show category name', ()=>{
    const transaction = CreateTransaction();
    const onSelect = jest.fn();
    
    const { getByText } = render(
      <TransactionListItem transaction={transaction} onSelect={onSelect}/>
    );

    const transactionCategoryName = transaction.category?.name ?? '';
    const categoryField = getByText(transactionCategoryName,{ exact: false });
    expect(categoryField).toBeInTheDocument();
  });
});

describe('on select', () =>{
  it('should call onSelect method', () =>{
    const transaction = CreateTransaction();
    const onSelect = jest.fn();

    const { getByText } = render(
      <TransactionListItem transaction={transaction} onSelect={onSelect}/>
    );

    act(()=>{
      const editButton = getByText('Editar');
      userEvent.click(editButton);
    });

    expect(onSelect).toBeCalledTimes(1);
    expect(onSelect).toBeCalledWith(transaction);
  });
});

describe('on remove', () =>{
  it('should call onRemove method', () =>{
    const transaction = CreateTransaction();
    const onRemove = jest.fn();

    const { getByText } = render(
      <TransactionListItem 
        transaction={transaction} 
        onSelect={jest.fn()} 
        onRemove={onRemove}
      />
    );

    act(()=>{
      const editButton = getByText('Remover');
      userEvent.click(editButton);
    });

    expect(onRemove).toBeCalledTimes(1);
    expect(onRemove).toBeCalledWith(transaction.id);
  });
});
