import { faker } from '@faker-js/faker';
import { Account } from '../../commom/interfaces/account';

type AccountBuilderArgs = {
  id?: string, 
  name?: string,
  description?: string, 
  currency?: string,
  isActive?: boolean
}

export function CreateAccount(args?:AccountBuilderArgs) : Account {
  return {
    id:           args?.id          ?? faker.datatype.uuid(),
    name:         args?.name        ?? faker.finance.accountName(),
    description:  args?.description ?? faker.finance.currencyCode(),
    currency:     args?.currency    ?? faker.finance.currencyCode(),
    isActive:     args?.isActive    ?? faker.datatype.boolean()
  } as Account;
}

export function CreateAccounts(args:AccountBuilderArgs) : Account[]{
  const count = faker.datatype.number(
    {
      min: 1,
      max: 10
    }
  );

  return Array.from(
    { length: count }, 
    () => CreateAccount(args)
  );
}