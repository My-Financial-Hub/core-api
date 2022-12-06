import { Transaction, TransactionType } from '../../../../interfaces/transaction';

interface TransactionListItemProps {
  transaction: Transaction,
  onSelect: (transaction: Transaction) => void,
  onRemove?: (id?: string) => void,
}

export default function TransactionListItem({transaction, onSelect, onRemove}:TransactionListItemProps){
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
          <p>{transaction.category?.name}</p>
          <p>{`${transaction.account?.currency??'$'}${transaction.amount.toFixed(2)}`}</p>
        </div>
        <div>
          <p>{transaction.description}</p>
        </div>
        <div>
          <button onClick={() => onSelect(transaction)}>Editar</button>
          <button onClick={() => onRemove?.(transaction.id)}>Remover</button>
        </div>
      </div>
    </li>
  );
}