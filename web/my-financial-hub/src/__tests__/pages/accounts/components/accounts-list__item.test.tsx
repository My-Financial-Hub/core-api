import { fireEvent, render } from '@testing-library/react';
import { faker } from '@faker-js/faker';

import {CreateAccount} from '../../../../__mocks__/account-builder';

import { AccountsProvider } from '../../../../pages/accounts/contexts/accounts-page-context';

import AccountListItem from '../../../../pages/accounts/components/accounts-list/account-list-item';

import * as hooks from '../../../../pages/accounts/hooks/accounts-page.hooks';

describe('on render', () =>{
  it('should render with all values', ()=>{
    const account = CreateAccount({});
    const { getByText } = render(<AccountListItem account={account}/>);

    expect(getByText(account.name)).toBeVisible();
    expect(getByText(account.description)).toBeVisible();
    expect(getByText(account.currency)).toBeVisible();
    expect(getByText(account.isActive? 'yes' : 'no')).toBeVisible();
  });

  it('should render delete button',()=>{
    const account = CreateAccount({});
    const { getByText } = render(<AccountListItem account={account}/>);
    expect(getByText('Delete')).toBeVisible();
  });
});

function mockDeleteAccount(){
  const randTimeOut = faker.datatype.number(
    {
      min:500,
      max:5000
    }
  );

  return jest.spyOn(hooks, 'useDeleteAccount').mockImplementation(
    async (context) => {
      setTimeout(() => {
        const [state, setState] = context;
        setState(
          {
            ...state,
            account: 
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

describe('on click', () =>{
  it('should select the current account',()=>{
    const account = CreateAccount({});
    const { getByText } = render(
      <AccountsProvider>
        <AccountListItem account={account}/>
      </AccountsProvider>
    );

    const name = getByText(account.name);
    fireEvent.click(name);
  });

  it('should delete account',()=>{
    const account = CreateAccount({});

    const meth = mockDeleteAccount();

    const { getByText } = render(
      <AccountsProvider>
        <AccountListItem account={account}/>
      </AccountsProvider>
    );

    const deleteButton = getByText('Delete');
    fireEvent.click(deleteButton);

    expect(meth).toBeCalledTimes(1);
  });
});