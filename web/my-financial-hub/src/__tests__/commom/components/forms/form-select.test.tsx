import { render } from '@testing-library/react';
import userEvent from '@testing-library/user-event';

import { CreateSelectOptions } from '../../../../__mocks__/forms/select-option-builder';
import { getRandomItem } from '../../../../__mocks__/utils/array-utils';

import FormSelect from '../../../../commom/components/forms/form-select';

describe('on render', () => {
  describe('when does not have a start value',()=>{
    it('should show the placeholder', () => {
      const expectedResult = 'placeholder';
      const { getByText } = render(
        <FormSelect
          placeholder={expectedResult}
          disabled={false}
          options={[]}
        />
      );
      const val = getByText(expectedResult);
    
      expect(val).toBeInTheDocument();
      expect(val).toHaveTextContent(expectedResult);
    });
  });

  describe('when has a start value', ()=>{
    it('should show the value', ()=>{
      const options = CreateSelectOptions(5);
      const expectedResult = options[0].label;
      const { getByText } = render(
        <FormSelect
          value={expectedResult}
          disabled={false}
          options={options}
        />
      );
      const val = getByText(expectedResult);
    
      expect(val).toBeInTheDocument();
      expect(val).toHaveTextContent(expectedResult);
    });
  });
  
  describe('when onDelete is null',() => {
    it('should not show delete option',() => {
      const { queryByText } = render(
        <FormSelect
          disabled={false}
          options={[]}
        />
      );

      const res = queryByText('Delete');
      expect(res).not.toBeInTheDocument();
    });
  });

});

describe('on click', () => {

  describe('when enabled', () => {
    it('should open the option list', () => {
      const options = CreateSelectOptions();
      const placeholder = 'placeholder';

      const { queryByRole, getByText } = render(
        <FormSelect
          placeholder={placeholder}
          disabled={false}
          options={options}
        />
      );

      let listbox = queryByRole('listbox');
      expect(listbox).not.toBeInTheDocument();

      const button = getByText(placeholder);
      userEvent.click(button);

      listbox = queryByRole('listbox');
      expect(listbox).toBeInTheDocument();
    });
    it('should show all the options', () => {
      const options = CreateSelectOptions();
      const placeholder = 'placeholder';

      const { queryByRole, getByText } = render(
        <FormSelect
          placeholder={placeholder}
          disabled={false}
          options={options}
        />
      );

      const listbox = queryByRole('listbox');
      expect(listbox).not.toBeInTheDocument();

      const button = getByText(placeholder);
      userEvent.click(button);

      const childrenCount = queryByRole('listbox')?.childElementCount;
      expect(childrenCount).toBe(options.length);
    });
  });

  describe('when disabled', () => {
    it('should not open the option list', () => {
      const options = CreateSelectOptions();
      const placeholder = 'placeholder';

      const { queryByRole, getByText } = render(
        <FormSelect
          placeholder={placeholder}
          disabled={true}
          options={options}
        />
      );

      let listbox = queryByRole('listbox');
      expect(listbox).not.toBeInTheDocument();

      const button = getByText(placeholder);
      userEvent.click(button);

      listbox = queryByRole('listbox');
      expect(listbox).not.toBeInTheDocument();
    });
  });

});

describe('on select', () => {
  it('should set the selected value', () => {
    const options = CreateSelectOptions();
    const placeholder = 'placeholder';

    const { getByText } = render(
      <FormSelect
        placeholder={placeholder}
        disabled={false}
        options={options}
      />
    );

    const button = getByText(placeholder);
    userEvent.click(button);

    const randomOption = getRandomItem(options);

    const option = getByText(randomOption.label);
    userEvent.click(option);

    expect(button).toHaveTextContent(randomOption.label);
  });

  it('should call onChangeOption method', () => {
    const options = CreateSelectOptions();
    const placeholder = 'placeholder';
    const onChangeOption = jest.fn();

    const { getByText } = render(
      <FormSelect
        placeholder={placeholder}
        disabled={false}
        options={options}
        onChangeOption={onChangeOption}
      />
    );

    const button = getByText(placeholder);
    userEvent.click(button);

    const randomOption = getRandomItem(options);

    const option = getByText(randomOption.label);
    userEvent.click(option);

    expect(onChangeOption).toBeCalledTimes(1);
  });
});

describe('on delete', () => {
  it('should set the placeholder value', () => {
    const options = CreateSelectOptions();
    const placeholder = 'placeholder';

    const { getByText , getByTestId } = render(
      <FormSelect
        placeholder={placeholder}
        disabled={false}
        options={options}
        onDeleteOption={jest.fn()}
      />
    );

    const button = getByText(placeholder);
    userEvent.click(button);

    const randomOption = getRandomItem(options);

    const option = getByTestId('delete-' + randomOption.value);
    userEvent.click(option);

    expect(button).toHaveTextContent(placeholder);
  });

  it('should remove the selected option', () => {
    const options = CreateSelectOptions();
    const placeholder = 'placeholder';

    const { getByText , getByTestId, queryByText } = render(
      <FormSelect
        placeholder={placeholder}
        disabled={false}
        options={options}
        onDeleteOption={jest.fn()}
      />
    );

    const button = getByText(placeholder);
    userEvent.click(button);

    const randomOption = getRandomItem(options);
    expect(getByText(randomOption.label)).toBeInTheDocument();
    
    const option = getByTestId('delete-' + randomOption.value);
    userEvent.click(option);
    
    userEvent.click(button);
    expect(queryByText(randomOption.label)).not.toBeInTheDocument();
  });

  it('should call ondelete method', () => {
    const options = CreateSelectOptions();
    const placeholder = 'placeholder';
    const onDeleteOption = jest.fn();

    const { getByText , getByTestId } = render(
      <FormSelect
        placeholder={placeholder}
        disabled={false}
        options={options}
        onDeleteOption={onDeleteOption}
      />
    );

    const button = getByText(placeholder);
    userEvent.click(button);

    const randomOption = getRandomItem(options);

    const option = getByTestId('delete-' + randomOption.value);
    userEvent.click(option);

    expect(onDeleteOption).toBeCalledTimes(1);
  });
});