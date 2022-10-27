export type SelectEnum = {
  [key: number]: string
}

export function getEnumKeys(en: SelectEnum): number[]{
  return Object.keys(en)
    .filter(x => !isNaN(parseInt(x)))
    .map(x => parseInt(x));
}

export function getEnumValues(en: SelectEnum): string[]{
  return Object.keys(en).filter(x => isNaN(parseInt(x)));
}

export function enumToString(en: SelectEnum, value :number): string{
  return en[value];
}