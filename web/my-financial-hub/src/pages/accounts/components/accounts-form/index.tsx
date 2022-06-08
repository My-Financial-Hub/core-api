import { FormEvent, useState } from 'react';
import { Account } from '../../../../commom/interfaces/account';

interface AccountFormProps {
  account? :Account
}

function AccountsForm(props: AccountFormProps) {

  const [account, setAccount] = useState(props.account?? {} as Account);

  const submitAccount = function (event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    fetch('http://localhost:5000/accounts', {
      method: 'POST',
      headers: {
        'content-type': 'application/json'
      },
      body: JSON.stringify(account)
    })
      .then((x: Response) => x.json())
      .then(() => 
        setAccount(
          { id: '', name: '', description: '', currency: '', isActive: false }
        )
      );
  };

  const changeAccount = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newAccount = {
      ...account,
      [event.target.name]: event.target.value
    } as Account;
    setAccount(newAccount);
  };

  const changeAccountActive = (event: React.ChangeEvent<HTMLInputElement>) => {
    console.log(event.target.value);
    const newAccount = {
      ...account,
      isActive: event.target.value == 'on'
    } as Account;
    setAccount(newAccount);
  };

  return (
    <form onSubmit={(e) => submitAccount(e)} className="container mb-4">
      <label htmlFor='name'>Name</label>
      <input name='name' title='name'
        type='text'
        placeholder='Insert Account name'
        maxLength={50}
        required
        onChange={changeAccount}
      />

      <label htmlFor='description'>Description</label>
      <input name='description' title='description'
        type='text'
        placeholder='Insert Account description'
        maxLength={500}
        onChange={changeAccount}
      />

      <label htmlFor='currency'>Currency</label>
      <input name='currency' title='currency'
        type='text'
        placeholder='Insert Account currency'
        maxLength={50}
        onChange={changeAccount}
      />

      <label htmlFor='isActive'>Is Active</label>
      <input
        name='isActive' title='isActive'
        type='checkbox'
        onChange={changeAccountActive}
      />

      <input type='submit' />
    </form>
  );
}

export default AccountsForm;
