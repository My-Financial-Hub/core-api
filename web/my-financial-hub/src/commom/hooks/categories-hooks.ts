import { Dispatch, SetStateAction } from 'react';
import CategoryApi from '../http/category-api';
import { Category } from '../interfaces/category';

export async function useCreateCategory(category: Category, api: CategoryApi) {
  try {
    await api.PostAsync(category);
  } catch (error) {
    console.error(error);
  }
}

export async function useGetAccounts(setState:Dispatch<SetStateAction<Category[]>>,api: CategoryApi) {
  try {
    const accountsResult = await api.GetAllAsync();
    setState(accountsResult.data);
  } catch (error) {
    console.error(error);
  }
}