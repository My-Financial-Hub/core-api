import { fireEvent, render, screen, waitFor } from '@testing-library/react';

import { CreateAccount } from '../../../../__mocks__/types/account-builder';
import { mockUseCreateAccount, mockUseUpdateAccount } from '../../../../__mocks__/hooks/accounts-page.hook';

import { AccountsProvider } from '../../../../pages/accounts/contexts/accounts-page-context';
import { Account } from '../../../../commom/interfaces/account';

import AccountsForm from '../../../../pages/accounts/components/accounts-form';

function fieldHasValue(fieldName: string, value: string) {
  const field = screen.getByTitle(fieldName);
  expect(field).toHaveValue(value);
}

async function isFormClear() {
  const nameField = await screen.findByTitle('name');
  expect(nameField).toHaveValue('');

  const descriptionField = await screen.findByTitle('description');
  expect(descriptionField).toHaveValue('');

  const currencyField = await screen.findByTitle('currency');
  expect(currencyField).toHaveValue('');

  const isActiveField = await screen.findByTitle('isActive');
  expect(isActiveField).not.toBeChecked();
}

function renderForm(account: Account) {
  const state = {
    accounts: [] as Account[],
    account: account,
    hasError: false,
    error: {
      message: ''
    }
  };

  const component = render(
    <AccountsProvider defaultState={state}>
      <AccountsForm />
    </AccountsProvider>
  );

  const name = screen.getByTitle('name');
  const description = screen.getByTitle('description');
  const currency = screen.getByTitle('currency');
  const isActive = screen.getByTitle('isActive');
  const submitButton = screen.getByText('Submit');

  return {
    ...component,
    //maybe not a good idea
    fields: {
      name, description,
      currency, isActive
    },
    submitButton
  };
}

describe('on render', () => {
  it.each([
    'name',
    'currency',
    'description',
    'isActive'
  ])('should render %s field', (fieldTitle) => {
    render(<AccountsForm />);

    const input = screen.getByTitle(fieldTitle);
    expect(input).toBeInTheDocument();
  });

  it('should render all fields empty', async () => {
    render(<AccountsForm />);

    await isFormClear();
  });
});

describe('on account changes', () => {
  it('should change fields values', () => {
    const account = CreateAccount({ id: '' });
    const state = {
      accounts: [] as Account[],
      account: account,
      hasError: false,
      error: {
        message: ''
      }
    };

    render(
      <AccountsProvider defaultState={state}>
        <AccountsForm />
      </AccountsProvider>
    );

    fieldHasValue('name', account.name);
    fieldHasValue('description', account.description);
    fieldHasValue('currency', account.currency);

    const isActiveField = screen.getByTitle('isActive');
    if (account.isActive) {
      expect(isActiveField).toBeChecked();
    } else {
      expect(isActiveField).not.toBeChecked();
    }
  });
});

describe('on submit', () => {
  jest.useFakeTimers();

  describe('with account id', () => {
    it('should make a put request', () => {
      const account = CreateAccount({});
    
      const meth = mockUseUpdateAccount();
      
      const { submitButton } = renderForm(account);
      fireEvent.click(submitButton);

      expect(meth).toBeCalledTimes(1);
    });
  });

  describe('without account id', () => {
    it('should make a post request', () => {
      const account = CreateAccount({ id: '' });
    
      const meth = mockUseCreateAccount();
      
      const { submitButton } = renderForm(account);
      fireEvent.click(submitButton);

      expect(meth).toBeCalledTimes(1);
    });
  });

  it('should set all fields empty', async () => {
    const account = CreateAccount({ id: '' });

    mockUseCreateAccount();
    
    const { submitButton } = renderForm(account);
    fireEvent.click(submitButton);

    jest.runAllTimers();

    await isFormClear();
  });

  it('should focus name field', async () => {
    const account = CreateAccount({ id: '' });

    mockUseCreateAccount();
    
    const { submitButton, fields } = renderForm(account);
    fireEvent.click(submitButton);

    jest.runAllTimers();

    waitFor( () =>{
      expect(fields.name).toHaveFocus();
    });
  });
});
