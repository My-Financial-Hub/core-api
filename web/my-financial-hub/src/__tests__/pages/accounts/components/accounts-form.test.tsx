import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import { faker } from '@faker-js/faker';

import {CreateAccount} from '../../../../__mocks__/account-builder';

import { AccountsProvider } from '../../../../pages/accounts/contexts/accounts-page-context';

import AccountsForm from '../../../../pages/accounts/components/accounts-form';

import { Account } from '../../../../commom/interfaces/account';
import * as hooks from '../../../../pages/accounts/hooks/accounts-page.hooks';

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

function mockUpdateAccount(account?: Account){
  const randTimeOut = faker.datatype.number(
    {
      min:500,
      max:5000
    }
  );

  return jest.spyOn(hooks, 'useUpdateAccount').mockImplementation(
    async (context) => {
      setTimeout(() => {
        const [state, setState] = context;
        setState(
          {
            ...state,
            account: account ??
            {
              name: '',
              description: '',
              currency: '',
              isActive: false
            }
          }
        );

        Promise.resolve();
      }, randTimeOut);
    }
  );
}

function mockCreateAccount(account?: Account){
  const randTimeOut = faker.datatype.number(
    {
      min:500,
      max:5000
    }
  );

  return jest.spyOn(hooks, 'useCreateAccount').mockImplementation(
    async (context) => {
      setTimeout(() => {
        const [state, setState] = context;
        setState(
          {
            ...state,
            account: account ??
            {
              name: '',
              description: '',
              currency: '',
              isActive: false
            }
          }
        );

        Promise.resolve();
      }, randTimeOut);
    }
  );
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
    
      const meth = mockUpdateAccount();
      
      const { submitButton } = renderForm(account);
      fireEvent.click(submitButton);

      expect(meth).toBeCalledTimes(1);
    });
  });

  describe('without account id', () => {
    it('should make a post request', () => {
      const account = CreateAccount({ id: '' });
    
      const meth = mockCreateAccount();
      
      const { submitButton } = renderForm(account);
      fireEvent.click(submitButton);

      expect(meth).toBeCalledTimes(1);
    });
  });

  it('should set all fields empty', async () => {
    const account = CreateAccount({ id: '' });

    mockCreateAccount();
    
    const { submitButton } = renderForm(account);
    fireEvent.click(submitButton);

    jest.runAllTimers();

    await isFormClear();
  });

  it('should focus name field', async () => {
    const account = CreateAccount({ id: '' });

    mockCreateAccount();
    
    const { submitButton, fields } = renderForm(account);
    fireEvent.click(submitButton);

    jest.runAllTimers();

    waitFor( () =>{
      expect(fields.name).toHaveFocus();
    });
  });
});
