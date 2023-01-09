import { faker } from '@faker-js/faker';

export function getRandomInt(from = 10, to = 10) : number{
  return faker.datatype.number(
    {
      min: from,
      max: to
    }
  );
}

export function getRandomFloat(from = 10, to = 10) : number{
  return faker.datatype.float(
    {
      min: from,
      max: to,
      precision: 2
    }
  );
}

