import { useState } from 'react';

import { useApisContext } from '../../commom/contexts/api-context';

import { Category, defaultCategory } from '../../commom/interfaces/category';

import CategoryForm from '../../commom/components/categories/form/category-form';
import SelectOption from '../../commom/components/forms/form-select/types/select-option';
import HttpFormSelect from '../../commom/components/forms/form-select/http-form-select';

//TODO create APIFormSelect
export default function CategoriesPage() {
  const { categoriesApi } = useApisContext();
  
  const [categories,setCategories] = useState<Category[]>([]);
  const [selectedCategory, setCategory] = useState<Category>();
  const [isLoading, setLoading] = useState(false);

  const selectCategory = function(option?: SelectOption){
    if(!option){
      setCategory(defaultCategory);
      return;
    }

    const foundCategories = categories.filter(c => c.id == option.value);
    if(foundCategories.length > 0){
      setCategory(foundCategories[0]);
    }
  };

  const submitCategory = async function(category: Category){
    setLoading(true);
    const foundCategories = categories.filter(c => c.id == category.id);

    if(foundCategories.length > 0){
      const index = categories.findIndex(obj => obj.id == category.id);
      categories[index] = category;
      setCategories(categories);
    }else{
      setCategories([...categories, category]);
    }
    setLoading(false);
  };
  
  return (
    <div className='container'>
      <CategoryForm 
        formData={selectedCategory} 
        onSubmit={submitCategory}
      />
      <HttpFormSelect 
        api={categoriesApi}
        placeholder='Select a category'
        disabled={isLoading}
        onChangeOption={selectCategory}
      />
    </div>
  );
}