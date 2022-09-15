import { useState } from 'react';
import TransactionForm from '../../commom/components/transactions/form/transaction-form';
import { defaultTransaction } from '../../commom/interfaces/transaction';

export default function TransactionsPage(){
  const [isLoading, setLoading] = useState(false);

  const submitForm = ()=>{
    console.log('yay! it worked');
  };

  return (
    <div className='container'>
      <h1>Transactions</h1>
      <TransactionForm 
        formData={defaultTransaction}
        categories={[]}
        accounts={[]}
        onSubmit={submitForm}
      />
    </div>
  );
}