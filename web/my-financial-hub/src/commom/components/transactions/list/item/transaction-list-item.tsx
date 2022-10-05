import { Transaction, TransactionType } from '../../../../interfaces/transaction';

interface TransactionListItemProps {
  transaction: Transaction
}

export default function TransactionListItem({transaction}:TransactionListItemProps){
  const label = transaction.type == TransactionType.Earn? 'Earn' : 'Expense';
  return(
    <li>
      <div>
        <div>
          <h4>{`${transaction.finishDate.toLocaleDateString()} - ${label} in ${transaction.account?.name}`}</h4>
          <p>{`${transaction.account?.currency}${transaction.amount}`}</p>
        </div>
        <div>
          <p>{transaction.description}</p>
        </div>
        <div>

        </div>
        <p>{transaction.category?.name}</p>
      </div>
    </li>
  );
}