import { useState } from 'react';
import TransactionForm from '../../commom/components/transactions/form/transaction-form';
import TransactionList from '../../commom/components/transactions/list/transaction-list';
import { defaultTransaction, Transaction } from '../../commom/interfaces/transaction';

export default function TransactionsPage() {
  const [transaction,setTransactions] = useState<Transaction>(defaultTransaction);

  const submitForm = (tran: Transaction) => {
    console.log('yay! it worked');
  };

  const selectTransaction = (tran: Transaction) => {
    setTransactions(tran);
  };

  return (
    <div className='container'>
      <h1>Transactions</h1>
      <TransactionForm
        formData={transaction}
        onSubmit={submitForm}
      />
      <TransactionList 
        onSelect={selectTransaction}
      />
    </div>
  );
}