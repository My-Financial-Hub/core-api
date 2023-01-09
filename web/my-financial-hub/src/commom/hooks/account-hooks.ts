import { useEffect, useState } from 'react';
import { useApisContext } from '../contexts/api-context';
import { Account } from '../interfaces/account';

export function useGetAccounts(): Account[]{//TODO: return screen result or something
  const [accounts,setAccounts] = useState<Account[]>([]);
  const {accountsApi} = useApisContext();

  const fetchAccounts = async function(){
    const result = await accountsApi.GetAllAsync();
    setAccounts(result.data);
  };

  useEffect(
    ()=>{
      fetchAccounts();
    }
  );
  return accounts;
}
