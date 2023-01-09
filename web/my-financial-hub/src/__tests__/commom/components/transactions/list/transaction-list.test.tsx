import { render, screen } from '@testing-library/react';

import { act } from 'react-dom/test-utils';
import userEvent from '@testing-library/user-event';

import { CreateTransactions } from '../../../../../__mocks__/types/transaction-builder';

import TransactionList from '../../../../../commom/components/transactions/list/transaction-list';
import { MockUseGetTransactions } from '../../../../../__mocks__/hooks/transactions-hooks';

describe('on render', () => {
  beforeEach(
    () => {
      jest.useFakeTimers('modern');
    }
  );

  afterEach(() => {
    jest.useRealTimers();
  });
  describe('with data', () => {
    it('should show all items', async () => {
      const transactions = CreateTransactions();
      const timeout = 100;
      MockUseGetTransactions(transactions, timeout);

      act(
        () => {
          render(
            <TransactionList />
          );
          jest.advanceTimersByTime(timeout + 1);
        }
      );

      const transactionList = await screen.findByTestId('transaction-list');
      expect(transactionList).toBeInTheDocument();
      expect(transactionList.children).toHaveLength(transactions.length);
    });

    it('should make a get request', async () => {
      const transactions = CreateTransactions();
      const timeout = 100;
      const onSelect = jest.fn();
      MockUseGetTransactions(transactions, timeout);

      act(
        () => {
          render(
            <TransactionList onSelect={onSelect} />
          );
          jest.advanceTimersByTime(timeout + 1);
        }
      );

    });
  });
  describe('without data', () => {
    it('should show empty message', async () => {
      const timeout = 100;
      MockUseGetTransactions([], timeout);

      act(
        () => {
          render(
            <TransactionList />
          );
          jest.advanceTimersByTime(timeout + 1);
        }
      );

      const emptyMessage = await screen.findByText('No transactions');
      expect(emptyMessage).toBeInTheDocument();
    });
  });
});

describe('on loading', () => {
  beforeEach(
    () => {
      jest.useFakeTimers('modern');
    }
  );

  afterEach(() => {
    jest.useRealTimers();
  });
  it('should render loading component', async () => {
    const timeout = 100;
    MockUseGetTransactions([], timeout);

    act(
      () => {
        render(
          <TransactionList />
        );
        jest.advanceTimersByTime(timeout);
      }
    );

    const emptyMessage = await screen.findByText('LOADING...');
    expect(emptyMessage).toBeInTheDocument();
  });
});

describe('on select', () => {
  beforeEach(
    () => {
      jest.useFakeTimers('modern');
    }
  );

  afterEach(() => {
    jest.useRealTimers();
  });
  it('should call OnSelect method', async () => {
    const transactions = CreateTransactions();
    const onSelect = jest.fn();
    MockUseGetTransactions(transactions);

    act(
      () => {
        render(
          <TransactionList onSelect={onSelect} />
        );
      }
    );

    const transactionList = await screen.findByTestId('transaction-list');
    const transactionSelect = transactionList.children[0];
    const transactionSelectButton = transactionSelect.querySelector('button');
    act(
      () => {
        if (transactionSelectButton) {
          userEvent.click(transactionSelectButton);
        }
      }
    );

    expect(onSelect).toBeCalled();
  });
  it('should call OnSelect method with the selected transaction', async () => {
    const transactions = CreateTransactions();
    const timeout = 100;
    const onSelect = jest.fn();
    MockUseGetTransactions(transactions, timeout);

    act(
      () => {
        render(
          <TransactionList onSelect={onSelect} />
        );
        jest.advanceTimersByTime(timeout + 1);
      }
    );

    const transactionList = await screen.findByTestId('transaction-list');
    const transactionSelect = transactionList.children[0];
    const transactionSelectButton = transactionSelect.querySelector('button');
    act(
      () => {
        if (transactionSelectButton) {
          userEvent.click(transactionSelectButton);
        }
        jest.advanceTimersByTime(1);
      }
    );

    expect(onSelect).toBeCalledWith(transactions[0]);
  });
});

describe('on change filter', () => {
  beforeEach(
    () => {
      jest.useFakeTimers('modern');
    }
  );

  afterEach(() => {
    jest.useRealTimers();
  });
  it('should render the filtered list', async () => {
    const transactions = CreateTransactions();      
    const timeout = 100;
    const hook = MockUseGetTransactions(transactions, 0);

    const filter = {
      startDate: new Date()
    };

    act(
      () => {
        const { rerender } = render(
          <TransactionList/>
        );
        jest.advanceTimersByTime(timeout + 1);
        
        rerender(
          <TransactionList filter={filter} />
        );
        jest.advanceTimersByTime(timeout + 1);
        
        
        filter.startDate = new Date(2020, 1, 1);
        rerender(
          <TransactionList filter={{
            startDate: new Date()
          }} />
        );
        jest.advanceTimersByTime(timeout + 1);
      }
    );

    expect(hook).toBeCalledTimes(2);
  });

  it('should call get transactions hook again', () => {
    const transactions = CreateTransactions();
    const timeout = 100;
    const hook = MockUseGetTransactions(transactions, timeout);

    act(
      () => {
        render(
          <TransactionList />
        );
        jest.advanceTimersByTime(timeout + 1);
      }
    );
    expect(hook).toBeCalledTimes(2);
  });
});