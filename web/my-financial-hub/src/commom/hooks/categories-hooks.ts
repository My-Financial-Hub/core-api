import CategoryApi from '../http/category-api';
import { Category } from '../interfaces/category';

export async function useCreateCategory(category: Category, api: CategoryApi) {
  try {
    await api.PostAsync(category);
  } catch (error) {
    console.error(error);
  }
}