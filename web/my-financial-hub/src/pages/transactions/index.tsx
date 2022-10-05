import TransactionForm from '../../commom/components/transactions/form/transaction-form';
import TransactionList from '../../commom/components/transactions/list/transaction-list';
import { defaultTransaction, Transaction } from '../../commom/interfaces/transaction';

export default function TransactionsPage() {
  const submitForm = (transaction: Transaction) => {
    console.log('yay! it worked');
  };

  return (
    <div className='container'>
      <h1>Transactions</h1>
      <TransactionForm
        formData={defaultTransaction}
        onSubmit={submitForm}
      />
      <TransactionList />
    </div>
  );
}