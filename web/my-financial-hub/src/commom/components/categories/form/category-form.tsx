import { FormEvent, useState } from 'react';
import { useApisContext } from '../../../contexts/api-context';
import { useCreateCategory } from '../../../hooks/categories-hooks';
import { Category, defaultCategory } from '../../../interfaces/category';
import FormFieldLabel from '../../forms/form-field';

//TODO: add callback
export default function CategoryForm() {
  const [category, setCategory] = useState<Category>(defaultCategory);
  const [isLoading, setLoading] = useState(false);

  const { categoriesApi } = useApisContext();

  const submitCategory = async function (event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setLoading(true);

    await useCreateCategory(category,categoriesApi);

    setLoading(false);
    setCategory(defaultCategory);
  };

  return (
    <form onSubmit={submitCategory}>
      <div className='row my-2'>
        <FormFieldLabel name='name' title='Name'>
          <input title='Name'
            type='text'
            placeholder='Insert Category Name'
            maxLength={500}
            value={category.name}
            onChange={
              (event) => setCategory({
                ...category,
                name: event.target.value
              })
            }
          />
        </FormFieldLabel>
      </div>

      <div className='row my-2'>
        <FormFieldLabel name='description' title='Description'>
          <textarea
            title='Description'
            value={category.description}
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
      <button disabled={isLoading} type='submit'>{!isLoading? 'Submit' : 'Loading'}</button>
    </form>
  );
}