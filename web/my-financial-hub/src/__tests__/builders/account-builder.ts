import { faker } from '@faker-js/faker';
import { Account } from '../../commom/interfaces/account';

export default function CreateAccount(
  {
    id, name, description,
    currency, isActive
  }:{
    id?: string, name?: string,
    description?: string, currency?: string,
    isActive?: boolean
  }
) : Account{
  return {
    id:           id          ?? faker.datatype.uuid(),
    name:         name        ?? faker.finance.accountName(),
    description:  description ?? faker.finance.currencyCode(),
    currency:     currency    ?? faker.finance.currencyCode(),
    isActive:     isActive    ?? faker.datatype.boolean()
  } as Account;
}