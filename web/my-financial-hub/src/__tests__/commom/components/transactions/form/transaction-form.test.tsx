import { act, render } from '@testing-library/react';
import userEvent from '@testing-library/user-event';

import TransactionForm from '../../../../../commom/components/transactions/form/transaction-form';

import { CreateTransaction } from '../../../../../__mocks__/types/transaction-builder';
import { TransactionStatus } from '../../../../../commom/interfaces/transaction';
import { MockUseCreateTransaction, MockUseUpdateTransaction } from '../../../../../__mocks__/hooks/transactions-hooks';

describe('on render', () => {
  describe('without data', () => {
    it('should render submit button with text "Create"', () => {
      const { getByText } = render(
        <TransactionForm />
      );

      const input = getByText('Create');
      expect(input).toBeInTheDocument();
    });
  });

  describe('with data', () => {
    it('should render submit button with text "Update"', () => {
      const transaction = CreateTransaction({ id: '1' });
      const { getByText } = render(
        <TransactionForm formData={transaction} />
      );

      const input = getByText('Update');
      expect(input).toBeInTheDocument();
    });
  });
});

describe('on submit', () => {
  beforeEach(
    () => {
      jest.useFakeTimers('modern');
    }
  );

  afterEach(() => {
    jest.useRealTimers();
  });

  describe('create transaction', () => {
    describe('with valid data', () => {
      it('should call "onSubmit" method', async () => {
        const onSubmit = jest.fn();
        const transaction = CreateTransaction();
        transaction.id = undefined;

        const timeout = 100;

        MockUseCreateTransaction(transaction, timeout);

        const { findByText } = render(
          <TransactionForm
            formData={transaction}
            onSubmit={onSubmit}
          />
        );

        await act(
          async () => {
            const input = await findByText('Create');
            userEvent.click(input);

            jest.advanceTimersByTime(timeout + 1);
          }
        );
        expect(onSubmit).toBeCalledTimes(1);
        expect(onSubmit).toBeCalledWith(transaction);
      });
      it('should call "UseCreateTransaction" hook', async () => {
        const transaction = CreateTransaction();
        transaction.id = undefined;

        const timeout = 100;

        const hookMock = MockUseCreateTransaction(transaction, timeout);

        const { findByText } = render(
          <TransactionForm
            formData={transaction}
          />
        );

        await act(
          async () => {
            const input = await findByText('Create');
            userEvent.click(input);

            jest.advanceTimersByTime(timeout + 1);
          }
        );
        expect(hookMock).toBeCalledTimes(1);
        expect(hookMock).toBeCalledWith(transaction, expect.anything());
      });
      it('should reset form', async () => {
        const onSubmit = jest.fn();
        const transaction = CreateTransaction();
        transaction.id = undefined;

        const timeout = 100;

        MockUseCreateTransaction(transaction, timeout);

        const { findByText, getByTitle, queryByTitle } = render(
          <TransactionForm
            formData={transaction}
            onSubmit={onSubmit}
          />
        );

        await act(
          async () => {
            const input = await findByText('Create');
            userEvent.click(input);

            jest.advanceTimersByTime(timeout + 1);
          }
        );

        //TODO: add validation to drodpowns
        // const typeInput = getByTitle('type');
        // expect(typeInput).toHaveValue('');

        const descriptionInput = getByTitle('description');
        expect(descriptionInput).toHaveValue('');

        // const categoryInput = getByTitle('category');
        // expect(categoryInput).toHaveValue('');

        // const accountInput = getByTitle('account');
        // expect(accountInput).toHaveValue('');

        const amountInput = getByTitle('amount');
        expect(amountInput).toHaveValue(1);

        const targetdateInput = getByTitle('targetdate');
        expect(targetdateInput).toHaveValue(new Date().toISOString().split('T')[0]);

        const finishdateInput = queryByTitle('finishdate');
        expect(finishdateInput).not.toBeInTheDocument();

        const isPaidInput = getByTitle('ispaid');
        expect(isPaidInput).not.toBeChecked();
      });
    });
    describe('with invalid data', () => {
      it('should not call "onSubmit" method', async () => {
        const onSubmit = jest.fn();
        const transaction = CreateTransaction({
          amount: -100
        });
        transaction.id = undefined;

        const timeout = 100;

        MockUseCreateTransaction(transaction, timeout);

        const { findByText } = render(
          <TransactionForm
            formData={transaction}
            onSubmit={onSubmit}
          />
        );

        await act(
          async () => {
            const input = await findByText('Create');
            userEvent.click(input);

            jest.advanceTimersByTime(timeout + 1);
          }
        );
        //https://www.youtube.com/watch?v=MhFSuOjU624&ab_channel=BrunoAntunes
        expect(onSubmit).not.toHaveBeenCalled();
      });
      it('should not call "UseCreateTransaction" hook', async () => {
        const transaction = CreateTransaction();
        transaction.id = undefined;

        const timeout = 100;

        const hookMock = MockUseCreateTransaction(transaction, timeout);

        const { findByText } = render(
          <TransactionForm />
        );

        await act(
          async () => {
            const input = await findByText('Create');
            userEvent.click(input);

            jest.advanceTimersByTime(timeout + 1);
          }
        );
        expect(hookMock).not.toHaveBeenCalled();
      });
      it('should not reset form', async () => {
        const onSubmit = jest.fn();
        const transaction = CreateTransaction({
          amount: -100,
          status: TransactionStatus.Committed
        });
        transaction.id = undefined;

        const timeout = 100;

        MockUseCreateTransaction(transaction, timeout);

        const { findByText, getByTitle, queryByTitle } = render(
          <TransactionForm
            formData={transaction}
            onSubmit={onSubmit}
          />
        );

        await act(
          async () => {
            const input = await findByText('Create');
            userEvent.click(input);

            jest.advanceTimersByTime(timeout + 1);
          }
        );

        //TODO: add validation to drodpowns
        // const typeInput = getByTitle('type');
        // expect(typeInput).toHaveValue('');

        const descriptionInput = getByTitle('description');
        expect(descriptionInput).toHaveValue(transaction.description);

        // const categoryInput = getByTitle('category');
        // expect(categoryInput).toHaveValue('');

        // const accountInput = getByTitle('account');
        // expect(accountInput).toHaveValue('');

        const amountInput = getByTitle('amount');
        expect(amountInput).toHaveValue(-100);

        const targetdateInput = getByTitle('targetdate');
        expect(targetdateInput).toHaveValue(transaction.targetDate);

        const finishdateInput = queryByTitle('finishdate');
        expect(finishdateInput).toHaveValue(transaction.finishDate);

        const isPaidInput = getByTitle('ispaid');
        expect(isPaidInput).toBeChecked();
      });
    });
  });

  describe('update transaction', () => {
    describe('with valid data', () => {
      it('should call "onSubmit" method', async () => {
        const onSubmit = jest.fn();
        const transaction = CreateTransaction();

        const timeout = 100;

        MockUseUpdateTransaction(transaction, timeout);

        const { findByText } = render(
          <TransactionForm
            formData={transaction}
            onSubmit={onSubmit}
          />
        );

        await act(
          async () => {
            const input = await findByText('Update');
            userEvent.click(input);

            jest.advanceTimersByTime(timeout + 1);
          }
        );
        expect(onSubmit).toBeCalledTimes(1);
        expect(onSubmit).toBeCalledWith(transaction);
      });
      it('should call "UseUpdateTransaction" hook', async () => {
        const transaction = CreateTransaction();
        const timeout = 100;

        const mockHook = MockUseUpdateTransaction(transaction, timeout);

        const { findByText } = render(
          <TransactionForm
            formData={transaction}
          />
        );

        await act(
          async () => {
            const input = await findByText('Update');
            userEvent.click(input);

            jest.advanceTimersByTime(timeout + 1);
          }
        );
        expect(mockHook).toBeCalledTimes(1);
        expect(mockHook).toBeCalledWith(transaction, expect.anything());
      });
      it('should reset form', async () => {
        const onSubmit = jest.fn();
        const transaction = CreateTransaction();

        const timeout = 100;

        MockUseUpdateTransaction(transaction, timeout);

        const { findByText, getByTitle, queryByTitle } = render(
          <TransactionForm
            formData={transaction}
            onSubmit={onSubmit}
          />
        );

        await act(
          async () => {
            const input = await findByText('Update');
            userEvent.click(input);

            jest.advanceTimersByTime(timeout + 1);
          }
        );

        //TODO: add validation to drodpowns
        // const typeInput = getByTitle('type');
        // expect(typeInput).toHaveValue('');

        const descriptionInput = getByTitle('description');
        expect(descriptionInput).toHaveValue('');

        // const categoryInput = getByTitle('category');
        // expect(categoryInput).toHaveValue('');

        // const accountInput = getByTitle('account');
        // expect(accountInput).toHaveValue('');

        const amountInput = getByTitle('amount');
        expect(amountInput).toHaveValue(1);

        const targetdateInput = getByTitle('targetdate');
        expect(targetdateInput).toHaveValue(new Date().toISOString().split('T')[0]);

        const finishdateInput = queryByTitle('finishdate');
        expect(finishdateInput).not.toBeInTheDocument();

        const isPaidInput = getByTitle('ispaid');
        expect(isPaidInput).not.toBeChecked();
      });
    });
    describe('with invalid data', () => {
      it('should not call "onSubmit" method', async () => {
        const onSubmit = jest.fn();
        const transaction = CreateTransaction({
          amount: -10
        });

        const timeout = 100;

        MockUseUpdateTransaction(transaction, timeout);

        const { findByText } = render(
          <TransactionForm
            formData={transaction}
            onSubmit={onSubmit}
          />
        );

        await act(
          async () => {
            const input = await findByText('Update');
            userEvent.click(input);

            jest.advanceTimersByTime(timeout + 1);
          }
        );
        expect(onSubmit).not.toHaveBeenCalled();
      });

      it('should not call "UseUpdateTransaction" hook', async () => {
        const transaction =  CreateTransaction({
          amount: -10
        });
        const timeout = 100;

        const mockHook = MockUseUpdateTransaction(transaction, timeout);

        const { findByText } = render(
          <TransactionForm
            formData={transaction}
          />
        );

        await act(
          async () => {
            const input = await findByText('Update');
            userEvent.click(input);

            jest.advanceTimersByTime(timeout + 1);
          }
        );
        expect(mockHook).not.toHaveBeenCalled();
      });
      it('should not reset form', async () => {
        const onSubmit = jest.fn();
        const transaction = CreateTransaction({
          amount: -100,
          isActive: true
        });

        const timeout = 100;

        MockUseUpdateTransaction(transaction, timeout);

        const { findByText, getByTitle, queryByTitle } = render(
          <TransactionForm
            formData={transaction}
            onSubmit={onSubmit}
          />
        );

        await act(
          async () => {
            const input = await findByText('Update');
            userEvent.click(input);

            jest.advanceTimersByTime(timeout + 1);
          }
        );

        //TODO: add validation to drodpowns
        // const typeInput = getByTitle('type');
        // expect(typeInput).toHaveValue('');

        const descriptionInput = getByTitle('description');
        expect(descriptionInput).toHaveValue(transaction.description);

        // const categoryInput = getByTitle('category');
        // expect(categoryInput).toHaveValue('');

        // const accountInput = getByTitle('account');
        // expect(accountInput).toHaveValue('');

        const amountInput = getByTitle('amount');
        expect(amountInput).toHaveValue(transaction.amount);

        const targetdateInput = getByTitle('targetdate');
        expect(targetdateInput).toHaveValue(transaction.targetDate);

        const finishdateInput = queryByTitle('finishdate');
        expect(finishdateInput).toHaveValue(transaction.finishDate);

        const isPaidInput = getByTitle('ispaid');
        expect(isPaidInput).toBeChecked();
      });
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
  it('should disable all fields', async () => {
    const transaction = CreateTransaction({
      isActive: true
    });

    const timeout = 100;

    MockUseUpdateTransaction(transaction, timeout);

    const { findByText, getByTitle, queryByTitle } = render(
      <TransactionForm
        formData={transaction}
      />
    );

    const input = await findByText('Update');
    userEvent.click(input);

    //TODO: add validation to drodpowns
    // const typeInput = getByTitle('type');
    // expect(typeInput).toBeDisabled();

    const descriptionInput = getByTitle('description');
    expect(descriptionInput).toBeDisabled();

    // const categoryInput = getByTitle('category');
    // expect(categoryInput).toBeDisabled();

    // const accountInput = getByTitle('account');
    // expect(accountInput).toBeDisabled();

    const amountInput = getByTitle('amount');
    expect(amountInput).toBeDisabled();

    const targetdateInput = getByTitle('targetdate');
    expect(targetdateInput).toBeDisabled();

    const finishdateInput = queryByTitle('finishdate');
    expect(finishdateInput).toBeDisabled();

    const isPaidInput = getByTitle('ispaid');
    expect(isPaidInput).toBeDisabled();
  });
  it('should disable the submit button', async () => {
    const transaction = CreateTransaction();

    const timeout = 100;

    MockUseUpdateTransaction(transaction, timeout);

    const { findByText } = render(
      <TransactionForm
        formData={transaction}
      />
    );

    const input = await findByText('Update');
    userEvent.click(input);
    expect(input).toBeDisabled();
  });
});
