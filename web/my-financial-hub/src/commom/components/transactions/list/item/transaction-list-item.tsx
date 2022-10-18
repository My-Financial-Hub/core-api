import { Transaction, TransactionType } from '../../../../interfaces/transaction';

interface TransactionListItemProps {
  transaction: Transaction,
  onSelect: (transaction: Transaction) => void
}

export default function TransactionListItem({transaction, onSelect}:TransactionListItemProps){
  const label = transaction.type == TransactionType.Earn? 'Earn' : 'Expense';

  /* 
  TODO: change curency to transaction.account?.currency
  TODO: add curency.js | https://currency.js.org/
  const formatter = Intl.NumberFormat('en-US',{
    style: 'currency',
    currency: 'USD',
  });
  */

  return(
    <li>
      <div>
        <div>
          <h4>{`${transaction.finishDate} - ${label} in ${transaction.account?.name}`}</h4>
          <p>{`${transaction.account?.currency??'$'}${transaction.amount.toFixed(2)}`}</p>
        </div>
        <div>
          <p>{transaction.description}</p>
        </div>
        <div>
          <button onClick={() => onSelect(transaction)}>Editar</button>
        </div>
        <p>{transaction.category?.name}</p>
      </div>
    </li>
  );
}