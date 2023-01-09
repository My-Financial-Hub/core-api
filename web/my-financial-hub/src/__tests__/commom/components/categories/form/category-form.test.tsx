import { render } from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import userEvent from '@testing-library/user-event';

import { CreateCategory } from '../../../../../__mocks__/types/category-builder';
import { MockUseCreateCategory } from '../../../../../__mocks__/hooks/categories-hook';

import CategoryForm from '../../../../../commom/components/categories/form/category-form';

describe('on render', () => {
  describe('default', () => {
    it.each([
      'name',
      'description',
      'isActive'
    ])('should render "%s" field', (fieldTitle: string) => {
      const { getByTitle } = render(
        <CategoryForm />
      );

      const input = getByTitle(fieldTitle);
      expect(input).toBeInTheDocument();
    });
  });

  describe('without formData', () => {
    it('should render submit button with text "Create"', () => {
      const { getByText } = render(
        <CategoryForm />
      );

      const input = getByText('Create');
      expect(input).toBeInTheDocument();
    });

    it('should render all fields empty', () => {
      const { getByTitle } = render(
        <CategoryForm />
      );

      const nameInput = getByTitle('name');
      expect(nameInput).toHaveValue('');

      const descriptionInput = getByTitle('description');
      expect(descriptionInput).toHaveValue('');

      const isActiveInput = getByTitle('isActive');
      expect(isActiveInput).not.toBeChecked();
    });
  });

  describe('with formData', () => {
    it('should render submit button with text "Update"', () => {
      const category = CreateCategory();
      const { getByText } = render(
        <CategoryForm formData={category} />
      );

      const input = getByText('Update');
      expect(input).toBeInTheDocument();
    });

    it('should fill the fields with data', () => {
      const category = CreateCategory();
      const { getByTitle } = render(
        <CategoryForm formData={category} />
      );

      const nameInput = getByTitle('name');
      expect(nameInput).toHaveValue(category.name);

      const descriptionInput = getByTitle('description');
      expect(descriptionInput).toHaveValue(category.description);

      const isActiveInput = getByTitle('isActive');
      if (category.isActive) {
        expect(isActiveInput).toBeChecked();
      } else {
        expect(isActiveInput).not.toBeChecked();
      }
    });
  });
});

describe('on submit', () => {

  beforeEach(
    () => {
      jest.useFakeTimers('modern');
    }
  );

  afterEach(() => {
    jest.useRealTimers();
  });

  describe('valid category', () => {
    it('should call "onSubmit" method', async () => {
      const onSubmit = jest.fn();
      const category = CreateCategory();
      category.id = undefined;

      const timeout = 100;
      MockUseCreateCategory(category, timeout);

      const { findByText } = render(
        <CategoryForm formData={category} onSubmit={onSubmit} />
      );

      await act(
        async ()=>{
          const input = await findByText('Create');
          userEvent.click(input);
    
          jest.advanceTimersByTime(timeout + 1);
        }
      );
      
      expect(onSubmit).toBeCalledTimes(1);
    });

    it('should reset form', async () => {
      const onSubmit = jest.fn();
      const category = CreateCategory();
      category.id = undefined;

      const timeout = 100;
      MockUseCreateCategory(category, timeout);

      const { findByText, getByTitle } = render(
        <CategoryForm formData={category} onSubmit={onSubmit} />
      );

      await act(
        async () => {
          const input = await findByText('Create');
          userEvent.click(input);
          jest.advanceTimersByTime(timeout + 1);
        }
      );

      const nameInput = getByTitle('name');
      expect(nameInput).toHaveValue('');

      const descriptionInput = getByTitle('description');
      expect(descriptionInput).toHaveValue('');

      const isActiveInput = getByTitle('isActive');
      expect(isActiveInput).not.toBeChecked();
    });
  });

  describe('invalid category', () => {
    it('should not call "onSubmit" method', () => {
      const onSubmit = jest.fn();
      const { getByText } = render(
        <CategoryForm onSubmit={onSubmit} />
      );

      act(
        () => {
          const input = getByText('Create');
          userEvent.click(input);
        }
      );

      expect(onSubmit).not.toHaveBeenCalled();
    });

    it('should not start the loading', async () => {
      const onSubmit = jest.fn();
      const category = CreateCategory();
      category.id = undefined;

      MockUseCreateCategory(category);

      const { findByText } = render(
        <CategoryForm formData={category} onSubmit={onSubmit} />
      );

      await act(
        async () => {
          const input = await findByText('Create');
          userEvent.click(input);
          expect(input).not.toBeDisabled();
        }
      );
    });
  });
});

describe('on loading', () => {
  it('should disable all fields', () => {
    const onSubmit = jest.fn();
    const category = CreateCategory();
    category.id = undefined;

    MockUseCreateCategory(category);

    const { getByText, getByTitle } = render(
      <CategoryForm formData={category} onSubmit={onSubmit} />
    );

    act(
      () => {
        const input = getByText('Create');
        userEvent.click(input);
      }
    );

    const nameInput = getByTitle('name');
    expect(nameInput).toBeDisabled();

    const descriptionInput = getByTitle('description');
    expect(descriptionInput).toBeDisabled();

    const isActiveInput = getByTitle('isActive');
    expect(isActiveInput).toBeDisabled();
  });
  it('should disable the submit button', () => {
    const onSubmit = jest.fn();
    const category = CreateCategory();
    category.id = undefined;

    MockUseCreateCategory(category);

    const { getByText } = render(
      <CategoryForm formData={category} onSubmit={onSubmit} />
    );

    const input = getByText('Create');
    act(
      () => {
        userEvent.click(input);
      }
    );

    expect(input).toBeDisabled();
  });
});