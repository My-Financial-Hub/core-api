import { getRandomInt } from './number-utils';

export function getRandomItem<T>(array :T[]) : T{
  const randomIndex = getRandomInt(0,array.length - 1);
  return array[randomIndex];
} 