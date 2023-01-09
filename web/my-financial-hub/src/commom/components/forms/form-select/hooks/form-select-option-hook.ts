import { useEffect, useState } from 'react';
import SelectOption from '../types/select-option';

interface IFormSelectOptionHookProps{
  value?: string,
  options: SelectOption[],
  onChangeOption?: (selectedOption?: SelectOption) => void,
  onDeleteOption?: (selectedOption?: string) => void, //TODO: maye move to another hook
}

interface IFormSelectOptionHook{
  selectedOption: number,
  optionsList: SelectOption[],
  selectOption: (option?: SelectOption) => void,
  deleteOption?: (option: string) => void,
}

export default function UseFormSelectOption(
  { 
    options, value,
    onChangeOption, onDeleteOption 
  } : IFormSelectOptionHookProps
) : IFormSelectOptionHook {
  const [optionsList, setOptionsList] = useState<SelectOption[]>(options);
  const [selectedOption, setSelectedOption] = useState(-1);

  const selectOption = function (option?: SelectOption) {
    const index = option === undefined ? -1 : optionsList.indexOf(option);
    setSelectedOption(index);
    onChangeOption?.(option);
  };

  const deleteOption = function (option: string) {
    setOptionsList(optionsList.filter(x => x.value != option));
    selectOption();
    onDeleteOption?.(option);
  };

  useEffect(() => {
    setOptionsList(options);
    
    const find = options.filter(x => x.value === value);
    const index = value === undefined ? -1 : optionsList.indexOf(find[0]);

    setSelectedOption(index);
  }, [options, optionsList ,value]); 

  useEffect(() => {
    const find = optionsList.filter(x => x.value === value);
    if (find.length > 0) {
      const index = optionsList.indexOf(find[0]);
      setSelectedOption(index);
    } else {
      setSelectedOption(-1);
    }
  }, [value, optionsList]);

  return {
    selectedOption,
    optionsList,
    selectOption,
    deleteOption
  };
}