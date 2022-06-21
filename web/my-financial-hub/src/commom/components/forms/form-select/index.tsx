import { useState } from 'react';
import FormSelectItem from './form-select-item';
import style from './form-select.module.scss';
import SelectOption from './types/select-option';

//https://react-select.com/components
export default function FormSelect(
  { options, onChangeOption }:
  { 
    options: SelectOption[], 
    onChangeOption: (selectedOption?: SelectOption) => void 
  }
) {
  const [isOpen, setOpen]                   = useState(false);
  const [optionsList, setOptionsList]       = useState<SelectOption[]>(options);
  const [selectedOption, setSelectedOption] = useState(-1);

  const selectOption = function(option?: SelectOption){
    const index = option === undefined? -1 : optionsList.indexOf(option);
    setSelectedOption(index);
    setOpen(false);
    onChangeOption(option);
  };

  const deleteOption = function(optionId: string){
    setOptionsList(optionsList.filter(x => x.value != optionId));
    selectOption();
  };

  return (
    <div>
      <div className={style.top}>
        <button
          type="button"
          aria-haspopup="listbox"
          aria-expanded={isOpen}
          className={isOpen ? 'expanded' : ''}
          onClick={() => setOpen(!isOpen)}
        >
          {selectedOption == -1 ? 'None' : optionsList[selectedOption].label}
        </button>
        <button onClick={() => selectOption()}>Clear</button>
      </div>

      <ul
        className={style[`options-body${isOpen ? '' : '--hiden'}`]}
        role="listbox"
        tabIndex={-1}
      >
        {
          optionsList.map(
            (option, index) => (
              <FormSelectItem 
                key={option.value} 
                option={option} 
                isSelected={selectedOption == index} 
                onSelect={selectOption} 
                onDelete={deleteOption} 
              />
            )
          )
        }
      </ul>
    </div>
  );
}