import { useEffect, useState } from 'react';
import TransactionForm from '../../commom/components/transactions/form/transaction-form';
import { useApisContext } from '../../commom/contexts/api-context';
import { useGetCategories } from '../../commom/hooks/categories-hooks';
import { Account } from '../../commom/interfaces/account';
import { Category } from '../../commom/interfaces/category';
import { defaultTransaction } from '../../commom/interfaces/transaction';

export default function TransactionsPage() {

  const { categoriesApi, accountsApi } = useApisContext();

  const [categories, setCategories] = useState<Category[]>([]);
  const [accounts, setAccounts] = useState<Account[]>([]);

  const submitForm = () => {
    console.log('yay! it worked');
  };

  const getAccounts = async function () {
    const accountsResult = await accountsApi.GetAllAsync();

    setAccounts(
      accountsResult.data
    );
  };

  const getCategories = async function () {
    setCategories(
      await useGetCategories(categoriesApi)
    );
  };

  useEffect(
    () => {
      getCategories();
      getAccounts();
    }
    , []
  );

  return (
    <div className='container'>
      <h1>Transactions</h1>
      <TransactionForm
        formData={defaultTransaction}
        categories={categories}
        accounts={accounts}
        onSubmit={submitForm}
      />
    </div>
  );
}