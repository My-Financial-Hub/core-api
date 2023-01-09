import { FormEvent, useEffect, useState } from 'react';
import { useApisContext } from '../../../contexts/api-context';
import { useCreateCategory, useUpdateCategory } from '../../../hooks/categories-hooks';
import { Category, defaultCategory } from '../../../interfaces/category';
import FormFieldLabel from '../../forms/form-field';

type FormProps ={
  formData? : Category,
  onSubmit? : (category:Category) => void
};
//TODO: use callback
export default function CategoryForm( 
  {formData = defaultCategory, onSubmit} : FormProps
) {
  const [category, setCategory] = useState<Category>(formData);
  const [isLoading, setLoading] = useState(false);

  const { categoriesApi } = useApisContext();

  const submitCategory = async function (event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setLoading(true);

    let cat : Category;

    if(category.id){
      await useUpdateCategory(category,categoriesApi);
      cat = category;
    }else{
      cat = await useCreateCategory(category,categoriesApi);
    }

    onSubmit?.(cat);
    setCategory(defaultCategory);
    setLoading(false);
  };

  useEffect(
    () =>{
      setCategory(formData);
    },
    [formData]
  );

  return (
    <form onSubmit={submitCategory}>
      <div className='row my-2'>
        <FormFieldLabel name='name' title='Name'>
          <input 
            title='name'
            type='text'
            placeholder='Insert Category Name'
            maxLength={500}
            value={category.name}
            disabled={isLoading}
            onChange={
              (event) => setCategory({
                ...category,
                name: event.target.value
              })
            }
            required={true}
          />
        </FormFieldLabel>
      </div>

      <div className='row my-2'>
        <FormFieldLabel name='description' title='Description'>
          <textarea
            title='description'
            value={category.description}
            disabled={isLoading}
            onChange={
              (event) => setCategory({
                ...category,
                description: event.target.value
              })
            }
          ></textarea>
        </FormFieldLabel>
      </div>
      
      <div className='row my-2'>
        <FormFieldLabel name='isActive' title='Is Active' direction='row'>
          <input
            className='checkbox'
            name='isActive' title='isActive'
            type='checkbox'
            disabled={isLoading}
            checked={category.isActive ?? false}
            onChange={
              () => setCategory({
                ...category,
                isActive: !category.isActive
              })
            }
          />
        </FormFieldLabel>
      </div>
      <button disabled={isLoading} type='submit'>{!isLoading? category.id? 'Update' : 'Create' : 'Loading'}</button>
    </form>
  );
}