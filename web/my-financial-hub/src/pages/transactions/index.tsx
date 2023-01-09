import { useState } from 'react';

import { defaultTransaction, Transaction } from '../../commom/interfaces/transaction';
import { TransactionFilter } from '../../commom/components/transactions/list/types/transaction-filter';

import TransactionForm from '../../commom/components/transactions/form/transaction-form';
import TransactionList from '../../commom/components/transactions/list/transaction-list';
import TransactionListFilter from '../../commom/components/transactions/list/filter/transaction-list-filter';

export default function TransactionsPage() {
  const [transaction,setTransactions] = useState<Transaction>(defaultTransaction);
  const [transactionFilter,setTransactionFilter] = useState<TransactionFilter>({} as TransactionFilter);

  const submitForm = function(tran: Transaction): void{
    setTransactions(defaultTransaction);
    setTransactionFilter({});
  };

  const selectTransaction = function(tran: Transaction): void{
    setTransactions(tran);
  };

  const filterTransactions = function(transactionFilter: TransactionFilter): void{
    setTransactionFilter(transactionFilter);
  };

  return (
    <div className='container'>
      <h1>Transactions</h1>
      <TransactionForm
        formData={transaction}
        onSubmit={submitForm}
      />
      <TransactionListFilter
        onFilter={filterTransactions}
      />
      <TransactionList 
        filter={transactionFilter}
        onSelect={selectTransaction}
      />
    </div>
  );
}