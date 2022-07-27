import * as hooks from '../../commom/hooks/categories-hooks';
import { Category } from '../../commom/interfaces/category';
import { getRandomInt } from '../commom/utils/number-utils';

const randTimeOut = getRandomInt(500, 5000);

export function mockUseGetCategories(categories?: Category[], timeout: number = randTimeOut) {
  return jest.spyOn(hooks, 'useGetCategories').mockImplementation(
    () => {
      return new Promise(
        () => {
          setTimeout(() => {
            if (categories) {
              return Promise.resolve(categories);
            } else {
              return Promise.reject([]);
            }
          }, timeout);
        }
      );
    }
  );
}

export function mockUseDeleteCategory() {
  return jest.spyOn(hooks, 'useDeleteCategory').mockImplementation(
    async () => {
      setTimeout(() => {
        Promise.resolve();
      }, randTimeOut);
    }
  );
}

export function mockUseUpdateCategory(account?: Category) {

  return jest.spyOn(hooks, 'useUpdateCategory').mockImplementation(
    async () => {
      setTimeout(() => {
        Promise.resolve(account);
      }, randTimeOut);
    }
  );
}

export function mockUseCreateCategory(category: Category, timeout: number = randTimeOut) {

  return jest.spyOn(hooks, 'useCreateCategory').mockImplementation(
    ()=>{
      jest.setTimeout(timeout);
      return Promise.resolve(category);
    }
  );
}