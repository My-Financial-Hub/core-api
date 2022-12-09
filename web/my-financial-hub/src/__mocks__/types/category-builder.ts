import { faker } from '@faker-js/faker';
import { Category } from '../../commom/interfaces/category';

type CategoryBuilderArgs = {
  id?: string, 
  name?: string,
  description?: string, 
  isActive?: boolean
}

export function CreateCategory(args? :CategoryBuilderArgs) : Category {
  return {
    id:           args?.id          ?? faker.datatype.uuid(),
    name:         args?.name        ?? faker.commerce.product(),
    description:  args?.description ?? faker.finance.currencyCode(),
    isActive:     args?.isActive    ?? faker.datatype.boolean()
  };
}

export function CreateCategories(args:CategoryBuilderArgs) : Category[]{
  const count = faker.datatype.number(
    {
      min: 1,
      max: 10
    }
  );

  return Array.from(
    { length: count }, 
    () => CreateCategory(args)
  );
}