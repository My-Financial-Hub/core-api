import { render } from '@testing-library/react';
import { act } from 'react-dom/test-utils';

import { getRandomInt } from '../../../../__mocks__/utils/number-utils';
import { CreateAccounts } from '../../../../__mocks__/types/account-builder';
import { mockUseGetAccounts } from '../../../../__mocks__/hooks/accounts-page.hook';

import { AccountsProvider } from '../../../../pages/accounts/contexts/accounts-page-context';

import AccountsList from '../../../../pages/accounts/components/accounts-list';

let randTimeOut = getRandomInt(500,5000);

beforeAll(
  () => {
    randTimeOut = getRandomInt(500,5000);
    jest.useFakeTimers();
  }
);

afterAll(
  () => {
    jest.useRealTimers();
  }
);

describe('on render', () => {
  describe('with accounts', () => {
    it.each([
      'Name', 'Description', 'Currency', 'Is Active'
    ])('should render %s header', async (name: string) => {
      const accounts = CreateAccounts({});
      mockUseGetAccounts(accounts,randTimeOut);

      const { findByText } = render(
        <AccountsProvider>
          <AccountsList />
        </AccountsProvider>
      );

      act(
        () => {
          jest.advanceTimersByTime(randTimeOut);
        }
      );
      const header = await findByText(name);
      expect(header).toBeInTheDocument();
      expect(header).toBeVisible();
    });

    it('should render all lines', async () => {
      const accounts = CreateAccounts({});
      mockUseGetAccounts(accounts,randTimeOut);

      const { findByTestId } = render(
        <AccountsProvider>
          <AccountsList />
        </AccountsProvider>
      );

      jest.advanceTimersByTime(randTimeOut);

      const content = await findByTestId('container-content');
      expect(content.children.length).toBe(accounts.length);
    });

    it('should get all accounts', async () => {
      const accounts = CreateAccounts({});
      const meth = mockUseGetAccounts(accounts,randTimeOut);

      act(
        () => {
          render(
            <AccountsProvider>
              <AccountsList />
            </AccountsProvider>
          );
        }
      );

      act(
        () => {
          jest.advanceTimersByTime(randTimeOut);
        }
      );

      expect(meth).toBeCalledTimes(1);
    });
  });

  describe('without accounts', () => {
    it('should render a "no accounts" message', async () => {
      mockUseGetAccounts([],randTimeOut);

      const { findByText } = render(
        <AccountsProvider>
          <AccountsList />
        </AccountsProvider>
      );

      act(
        () => {
          jest.advanceTimersByTime(randTimeOut);
        }
      );

      const loading = await findByText('No accounts');
      expect(loading).toBeInTheDocument();
      expect(loading).toBeVisible();
    });
  });
});

describe('on loading', () => {
  it('should render loading component', async () => {
    const accounts = CreateAccounts({});
    mockUseGetAccounts(accounts,randTimeOut);

    const { findByText } = render(
      <AccountsProvider>
        <AccountsList />
      </AccountsProvider>
    );

    const loading = await findByText('LOADING...');
    expect(loading).toBeInTheDocument();
    expect(loading).toBeVisible();
  });
});
