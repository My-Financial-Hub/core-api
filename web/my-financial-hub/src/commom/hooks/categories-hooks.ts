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

export async function useGetCategories(setState:Dispatch<SetStateAction<Category[]>>,api: CategoryApi) {
  try {
    const accountsResult = await api.GetAllAsync();
    setState(accountsResult.data);
  } catch (error) {
    console.error(error);
  }
}

export async function useDeleteCategory(setState:Dispatch<SetStateAction<Category[]>>,id: string,api: CategoryApi) {
  try {
    await api.DeleteAsync(id);
    setState(
      (oldState) => {
        console.log(oldState.filter(x => x.id !== id));
        return oldState.filter(x => x.id !== id);
      }
    );
  } catch (error) {
    console.error(error);
  }
}