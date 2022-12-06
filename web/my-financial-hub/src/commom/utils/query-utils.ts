export function createUrlQuery(queryName: string,query?:string[]): string{
  let result = '';
  if((query?.length ?? 0) > 0){
    result += `${queryName}=`;
    query?.forEach(
      (que)=> {
        result += `${que},`;
      }
    );
    result = result.substring(0,result.length - 1) + '&';
  }
  return result;
}

export function createUrlStartEndDateQuery(startDate?: Date, endDate?: Date): string{
  if(!startDate || !endDate){
    return '';
  }
  return `startDate=${startDate.toISODateString()}&targetDate=${endDate.toISODateString()}` + '&';
}

export function createUrlQueryNumber(queryName: string,nums?: number[]): string{
  let result = '';

  if((nums?.length ?? 0) > 0){
    result += `${queryName}=`;
    nums?.forEach(
      (num)=> {
        result += `${num},`;
      }
    );
    result = result.substring(0,result.length - 1) + '&';
  }
  
  return result;
}