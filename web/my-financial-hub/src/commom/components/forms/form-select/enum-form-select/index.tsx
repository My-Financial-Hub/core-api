import { useEffect, useState } from 'react';
import FormSelect from '..';
import { getEnumKeys, SelectEnum } from '../../../../utils/enum-utils';
import SelectOption from '../types/select-option';

type EnumFormSelectProps = {
  placeholder?: string,
  value?: number,
  disabled: boolean,
  options: SelectEnum,
  onChangeOption?: (selectedOption?: number) => void
}

const defaultValue = '-1';

/**
* @deprecated This component is not working (fix OnChangeOption)
**/
export default function EnumFormSelect(
  {
    options, value,
    disabled, placeholder = '',
    onChangeOption
  }:
  EnumFormSelectProps
) {
  const [optionsList, setOptionsList] = useState<SelectOption[]>([]);
  const [enumValue, setValue] = useState<string>(defaultValue);

  const changeOption = function (option?: SelectOption) {
    setValue(option?.value ?? defaultValue);
    onChangeOption?.(parseInt(enumValue));
  };

  useEffect(() => {
    const opts = getEnumKeys(options).map<SelectOption>(key =>
      ({
        label : options[key],
        value : key.toString()
      })
    );
    setOptionsList(opts);
  }, [options]);

  useEffect(() =>{
    setValue(value?.toString() ?? defaultValue);
  }, [value]);
  
  return (
    <FormSelect 
      options={optionsList}
      value={enumValue}
      disabled={disabled}
      placeholder={placeholder}
      onChangeOption={changeOption}
    />
  );
}