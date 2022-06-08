import { Account } from '../../../../../commom/interfaces/account';
import style from './account-list__item.module.scss';

interface AccountListItemProps {
  account: Account,
  onSelect: (account: Account) => void,
  onRemove: (id: string) => void
}

export default function AccountListItem({ account, onSelect, onRemove }: AccountListItemProps) {
  return (
    //${account.isActive?style.inactive:style.inactive}
    <div className={`row d-flex p-2 align-middle ${style.row}`} onClick={() => onSelect(account)}>
      <div className='col-3'>
        {account.name}
      </div>
      <div className='col-6'>
        {account.description}
      </div>
      <div className='col-1'>
        {account.currency}
      </div>
      <div className='col-1'>
        {account.isActive ? 'yes' : 'no'}
      </div>
      <div className='col-1'>
        <button onClick={()=> onRemove(account.id)}>Delete</button>
      </div>
    </div>
  );
} 