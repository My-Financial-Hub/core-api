import { render } from '@testing-library/react';
import Api from '../../../../commom/http/api';
import userEvent from '@testing-library/user-event';

import { CreateSelectOptions } from '../../../../__mocks__/forms/select-option-builder';

import HttpFormSelect from '../../../../commom/components/forms/form-select/http-form-select';
import { getRandomItem } from '../../../../__mocks__/utils/array-utils';

let api: Api<string>;
const baseUrl = 'hhtp://localhost:8000';
const baseEndpoint = 'test-base';

describe('on render', () => {
  afterAll(() =>{
    api = new Api<string>(baseUrl,baseEndpoint);
  }),
  describe('when does not have a start value',()=>{
    it('should show the placeholder', () => {
      const expectedResult = 'placeholder';
      const { getByText } = render(
        <HttpFormSelect
          api={api}
          placeholder={expectedResult}
          disabled={false}
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
        <HttpFormSelect
          api={api}
          value={expectedResult}
          disabled={false}
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
        <HttpFormSelect
          api={api}
          disabled={false}
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
      const placeholder = 'placeholder';

      const { queryByRole, getByText } = render(
        <HttpFormSelect
          api={api}
          placeholder={placeholder}
          disabled={false}
        />
      );

      let listbox = queryByRole('listbox');
      expect(listbox).not.toBeInTheDocument();

      const button = getByText(placeholder);
      userEvent.click(button);

      listbox = queryByRole('listbox');
      expect(listbox).toBeInTheDocument();
    });
    it('should call the get request', () => {
      const placeholder = 'placeholder';
      render(
        <HttpFormSelect
          api={api}
          placeholder={placeholder}
          disabled={false}  
          onDeleteOption={jest.fn()}
        />
      );
      
      expect(true).toBe(false);
    });
    it('should show all the options', () => {
      const options = CreateSelectOptions();
      const placeholder = 'placeholder';

      const { queryByRole, getByText } = render(
        <HttpFormSelect
          api={api}
          placeholder={placeholder}
          disabled={false}
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
      const placeholder = 'placeholder';

      const { queryByRole, getByText } = render(
        <HttpFormSelect
          api={api}
          placeholder={placeholder}
          disabled={true}
        />
      );

      let listbox = queryByRole('listbox');
      expect(listbox).not.toBeInTheDocument();

      const button = getByText(placeholder);
      userEvent.click(button);

      listbox = queryByRole('listbox');
      expect(listbox).not.toBeInTheDocument();
    });

    it('should call the on select', () => {
      const placeholder = 'placeholder';
      render(
        <HttpFormSelect
          api={api}
          placeholder={placeholder}
          disabled={true}
          onDeleteOption={jest.fn()}
        />
      );
      
      expect(true).toBe(false);
    });
  });

});

describe('on select', () => {
  it('should set the selected value on the default value', () => {
    const options = CreateSelectOptions();
    const placeholder = 'placeholder';

    const { getByText } = render(
      <HttpFormSelect
        api={api}
        placeholder={placeholder}
        disabled={false}
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
      <HttpFormSelect
        api={api}
        placeholder={placeholder}
        disabled={false}
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
      <HttpFormSelect
        api={api}
        placeholder={placeholder}
        disabled={false}
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
      <HttpFormSelect
        api={api}
        placeholder={placeholder}
        disabled={false}
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
  it('should call a delete request', () => {
    const placeholder = 'placeholder';
    render(
      <HttpFormSelect
        api={api}
        placeholder={placeholder}
        disabled={false}
        onDeleteOption={jest.fn()}
      />
    );
    
    expect(true).toBe(false);
  });
  it('should call ondelete method', () => {
    const options = CreateSelectOptions();
    const placeholder = 'placeholder';
    const onDeleteOption = jest.fn();

    const { getByText , getByTestId } = render(
      <HttpFormSelect
        api={api}
        placeholder={placeholder}
        disabled={false}
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