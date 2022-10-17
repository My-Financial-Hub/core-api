import { render } from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import userEvent from '@testing-library/user-event';

import TransactionForm from '../../../../../commom/components/transactions/form/transaction-form';

describe('on render', () =>{
  describe('default',() =>{

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
      const { getByText } = render(
        <TransactionForm />
      );

      const input = getByText('Update');
      expect(input).toBeInTheDocument();
    });
  });

});