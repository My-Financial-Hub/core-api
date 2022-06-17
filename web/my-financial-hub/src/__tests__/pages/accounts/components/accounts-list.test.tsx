import { faker } from '@faker-js/faker';
import { render } from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import { Account } from '../../../../commom/interfaces/account';
import AccountsList from '../../../../pages/accounts/components/accounts-list';
import { AccountsProvider } from '../../../../pages/accounts/contexts/accounts-page-context';
import * as hooks from '../../../../pages/accounts/hooks/accounts-page.hooks';
import { CreateAccounts } from '../../../../__mocks__/account-builder';

let randTimeOut = faker.datatype.number(
  {
    min: 500,
    max: 5000
  }
);

//https://kentcdodds.com/blog/fix-the-not-wrapped-in-act-warning
function mockGetAccounts(accounts?: Account[], timeout: number = randTimeOut) {
  return jest.spyOn(hooks, 'useGetAccounts').mockImplementation(
    async (context) => {
      setTimeout(() => {
        const [state, setState] = context;
        if (accounts) {
          setState({
            ...state,
            accounts
          });
          Promise.resolve();
        } else {
          Promise.reject('no accounts');
        }
      }, timeout);
    }
  );
}

beforeAll(
  () => {
    randTimeOut = faker.datatype.number(
      {
        min: 500,
        max: 5000
      }
    );
    jest.useFakeTimers();
  }
);

afterAll(
  () => {
    jest.useRealTimers();
  }
);

describe('on render', () => {
  it.each([
    'Name', 'Description', 'Currency', 'Is Active'
  ])('should render %s header', async (name: string) => {
    const accounts = CreateAccounts({});
    mockGetAccounts(accounts);

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
    mockGetAccounts(accounts);

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
    const meth = mockGetAccounts(accounts);

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

describe('on loading', () => {
  it('should render loading component', async () => {
    const accounts = CreateAccounts({});
    mockGetAccounts(accounts);

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

describe('without accounts', () => {
  it('should render a "no accounts" message', async () => {
    mockGetAccounts([]);

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