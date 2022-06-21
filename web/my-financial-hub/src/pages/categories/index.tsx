import { useState } from 'react';
import CategoryForm from '../../commom/components/categories/form/category-form';
import FormSelect from '../../commom/components/forms/form-select';
import SelectOption from '../../commom/components/forms/form-select/types/select-option';

export default function CategoriesPage() {
  const opt = [
    {
      id: 'aaa',
      name: 'aaa'
    },
    {
      id: 'bbb',
      name: 'bbb'
    }, 
    {
      id: 'ccc',
      name: 'ccc'
    }
  ];

  const [item, setItem] = useState<SelectOption>();
  return (
    <div className='container'>
      <CategoryForm />
      {item?.name}
      <FormSelect options={opt} onChangeOption={(e) => setItem(e)} />
    </div>
  );
}