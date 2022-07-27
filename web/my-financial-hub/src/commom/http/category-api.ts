/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { Category } from '../interfaces/category';
import Api from './api';

export default class CategoryApi extends Api<Category>{
  constructor(){
    super(process.env.REACT_APP_API_BASE_URL!, 'categories');
  }
}