import { fireEvent, render } from '@testing-library/react';

import { CreateAccount } from '../../../../__mocks__/types/account-builder';
import { mockUseDeleteAccount } from '../../../../__mocks__/hooks/accounts-page.hook';

import { AccountsProvider } from '../../../../pages/accounts/contexts/accounts-page-context';

import AccountListItem from '../../../../pages/accounts/components/accounts-list/account-list-item';

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

    const meth = mockUseDeleteAccount();

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