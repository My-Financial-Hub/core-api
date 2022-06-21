export interface ServiceResultError{
  code:number,
  message:string,
}

export interface ServiceResult<T>{
  hasError:boolean,
  error:ServiceResultError,
  data: T
}