import { render } from '@testing-library/react';

import TransactionForm from '../../../../../commom/components/transactions/form/transaction-form';
import { CreateTransaction } from '../../../../../__mocks__/types/transaction-builder';

describe('on render', () =>{
  describe('default',() =>{
    render(
      <TransactionForm />
    );
    expect(true).toBe(false);
  });

  describe('without data',()=>{
    it('should render submit button with text "Create"', () => {
      const { getByText } = render(
        <TransactionForm />
      );

      const input = getByText('Create');
      expect(input).toBeInTheDocument();
    });
  });

  describe('with data',()=>{
    it('should render submit button with text "Update"', () => {
      const transaction = CreateTransaction({id: '1'});
      const { getByText } = render(
        <TransactionForm formData={transaction}/>
      );

      const input = getByText('Update');
      expect(input).toBeInTheDocument();
    });
  });

});

describe('on submit',()=>{
  describe('create transaction',()=>{
    describe('with valid data',()=>{
      it('should call "onSubmit" method', ()=>{
        render(
          <TransactionForm />
        );
        expect(true).toBe(false);
      });

      it('should send a POST request', ()=>{
        render(
          <TransactionForm />
        );
        expect(true).toBe(false);
      });
      it('should reset form', () => {
        render(
          <TransactionForm />
        );
        expect(true).toBe(false);
      });
    });
    describe('with invalid data',()=>{
      it('should not call "onSubmit" method', ()=>{
        render(
          <TransactionForm />
        );
        expect(true).toBe(false);
      });

      it('should not send a POST request', ()=>{
        render(
          <TransactionForm />
        );
        expect(true).toBe(false);
      });
      it('should not reset form', () => {
        render(
          <TransactionForm />
        );
        expect(true).toBe(false);
      });
    });
  });
  describe('update transaction',()=>{
    describe('with valid data',()=>{
      it('should call "onSubmit" method', ()=>{
        render(
          <TransactionForm />
        );
        expect(true).toBe(false);
      });

      it('should send a PUT request', ()=>{
        render(
          <TransactionForm />
        );
        expect(true).toBe(false);
      });
      it('should reset form', () => {
        render(
          <TransactionForm />
        );
        expect(true).toBe(false);
      });
    });
    describe('with invalid data',()=>{
      it('should not call "onSubmit" method', ()=>{
        render(
          <TransactionForm />
        );
        expect(true).toBe(false);
      });

      it('should not send a PUT request', ()=>{
        render(
          <TransactionForm />
        );
        expect(true).toBe(false);
      });
      it('should not reset form', () => {
        render(
          <TransactionForm />
        );
        expect(true).toBe(false);
      });
    });
  });
});

describe('on loading',()=>{
  it('should disable all fields', () => {
    render(
      <TransactionForm />
    );
    expect(true).toBe(false);
  });
  it('should disable the submit button', () => {
    render(
      <TransactionForm />
    );
    expect(true).toBe(false);
  });
});