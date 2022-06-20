export interface Account{
  id?: string,
  name: string,
  description: string,
  currency: string,
  isActive: boolean
}

export const defaultAccount: Account = { 
  name: '', 
  description: '', 
  currency: '', 
  isActive: false 
};