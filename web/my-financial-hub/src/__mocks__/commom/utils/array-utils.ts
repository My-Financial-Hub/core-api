import { getRandomInt } from './number-utils';

export function getRandomItem<T>(array :T[]) : T{
  const randomIndex = getRandomInt(0,array.length);
  return array[randomIndex];
} 