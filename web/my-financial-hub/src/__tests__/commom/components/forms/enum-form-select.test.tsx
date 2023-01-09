import { render } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import EnumFormSelect from '../../../../commom/components/forms/form-select/enum-form-select';
import { getEnumKeys } from '../../../../commom/utils/enum-utils';

enum TestEnum{
  val=9,
  sec=2,
  tes=4,
  frst=1
}

describe('on render', ()=>{
  it('should show the first item of the enum', ()=>{
    const expectedResult = TestEnum.frst.toString();
    const { getByText } = render(
      <EnumFormSelect 
        options={TestEnum}
        disabled={false}
      />
    );
    const val = getByText(expectedResult);
    
    expect(val).toBeInTheDocument();
    expect(val).toHaveTextContent(expectedResult);
  });
});

describe('on click', () => {

  describe('when enabled', () => {
    it('should open the option list', () => {
      const placeholder = 'placeholder';

      const { queryByRole, getByText } = render(
        <EnumFormSelect
          placeholder={placeholder}
          disabled={false}
          options={TestEnum}
        />
      );

      let listbox = queryByRole('listbox');
      expect(listbox).not.toBeInTheDocument();

      const button = getByText(TestEnum.frst.toString());
      userEvent.click(button);

      listbox = queryByRole('listbox');
      expect(listbox).toBeInTheDocument();
    });
    it('should show all the options', () => {
      const keys = getEnumKeys(TestEnum);

      const { queryByRole, getByText } = render(
        <EnumFormSelect
          disabled={false}
          options={TestEnum}
        />
      );

      const listbox = queryByRole('listbox');
      expect(listbox).not.toBeInTheDocument();

      const button = getByText(TestEnum.frst.toString());
      userEvent.click(button);

      const childrenCount = queryByRole('listbox')?.childElementCount;
      expect(childrenCount).toBe(keys.length);
    });
  });

  describe('when disabled', () => {
    it('should not open the option list', () => {
      const { queryByRole, getByText } = render(
        <EnumFormSelect
          disabled={true}
          options={TestEnum}
        />
      );

      let listbox = queryByRole('listbox');
      expect(listbox).not.toBeInTheDocument();

      const button = getByText(TestEnum.frst.toString());
      userEvent.click(button);

      listbox = queryByRole('listbox');
      expect(listbox).not.toBeInTheDocument();
    });
  });

});