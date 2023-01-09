import { faker } from '@faker-js/faker';
import SelectOption from '../../commom/components/forms/form-select/types/select-option';

export function CreateSelectOption() : SelectOption {
  return {
    value: faker.datatype.uuid(),
    label: faker.word.noun()
  };
}

export function CreateSelectOptions(length?: number) : SelectOption[] {
  length ??= faker.datatype.number({min: 1, max: 10});

  return Array.from(
    { length }, 
    () => CreateSelectOption()
  );
}