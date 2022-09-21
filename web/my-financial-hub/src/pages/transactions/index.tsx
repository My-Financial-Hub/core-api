import TransactionForm from '../../commom/components/transactions/form/transaction-form';
import { defaultTransaction } from '../../commom/interfaces/transaction';

export default function TransactionsPage() {
  const submitForm = () => {
    console.log('yay! it worked');
  };

  return (
    <div className='container'>
      <h1>Transactions</h1>
      <TransactionForm
        formData={defaultTransaction}
        onSubmit={submitForm}
      />
    </div>
  );
}