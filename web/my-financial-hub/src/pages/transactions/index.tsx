import { useState } from 'react';

import { defaultTransaction, Transaction } from '../../commom/interfaces/transaction';
import { TransactionFilter } from '../../commom/components/transactions/list/types/transaction-filter';

import { UseDeleteTransaction } from '../../commom/hooks/transactions-hooks';
import { useApisContext } from '../../commom/contexts/api-context';

import TransactionForm from '../../commom/components/transactions/form/transaction-form';
import TransactionList from '../../commom/components/transactions/list/transaction-list';
import TransactionListFilter from '../../commom/components/transactions/list/filter/transaction-list-filter';

export default function TransactionsPage() {
  const [transaction,setTransactions] = useState<Transaction>(defaultTransaction);
  
  const submitForm = (tran: Transaction) => {
    console.log('yay! it worked');
  };

  const selectTransaction = (tran: Transaction) => {
    setTransactions(tran);
  };

  const filterTransactions = function(transactionFilter: TransactionFilter){
    console.log('yay! it worked');
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
        onSelect={selectTransaction}
      />
    </div>
  );
}