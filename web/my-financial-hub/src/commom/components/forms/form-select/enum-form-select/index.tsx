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

export default function EnumFormSelect(
  {
    options, value,
    disabled, placeholder = '',
    onChangeOption
  }:
  EnumFormSelectProps
) {
  const [optionsList, setOptionsList] = useState<SelectOption[]>([]);

  const changeOption = function (option?: SelectOption) {
    onChangeOption?.(parseInt(option?.value?? '-1'));
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

  
  return (
    <FormSelect 
      options={optionsList}
      value={value?.toString()}
      disabled={disabled}
      placeholder={placeholder}
      onChangeOption={changeOption}
    />
  );
}