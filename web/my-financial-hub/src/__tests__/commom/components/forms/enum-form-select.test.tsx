import { render } from '@testing-library/react';
import EnumFormSelect from '../../../../commom/components/forms/form-select/enum-form-select';

enum TestEnum{
  val=9,
  sec=2,
  tes=4,
  ot=1
}

describe('on render', ()=>{
  it('should show the first item of the enum', ()=>{
    const expectedResult = TestEnum.ot.toString();
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