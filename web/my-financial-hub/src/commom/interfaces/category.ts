export interface Category{
  id?: string,
  name: string,
  description: string,
  isActive: boolean
}

export const defaultCategory: Category = { 
  name: '', 
  description: '', 
  isActive: false 
};