import { useEffect, useState } from 'react';
import FormSelectItem from './form-select-item';
import style from './form-select.module.scss';
import SelectOption from './types/select-option';

type FormSelectProps = {
  placeholder?: string,
  disabled: boolean,
  options: SelectOption[]
  onChangeOption?: (selectedOption?: SelectOption) => void,
  onDeleteOption?: (selectedOption?: string) => void,
}

//https://react-select.com/components
export default function FormSelect(
  {
    options, disabled, placeholder = '',
    onChangeOption, onDeleteOption
  }:
    FormSelectProps
) {
  const [isOpen, setOpen] = useState(false);
  const [optionsList, setOptionsList] = useState<SelectOption[]>(options);
  const [selectedOption, setSelectedOption] = useState(-1);

  const selectOption = function (option?: SelectOption) {
    const index = option === undefined ? -1 : optionsList.indexOf(option);
    setSelectedOption(index);
    setOpen(false);
    onChangeOption?.(option);
  };

  const deleteOption = function (value: string) {
    setOptionsList(optionsList.filter(x => x.value != value));
    selectOption();
    onDeleteOption?.(value);
  };

  useEffect(() => {
    setOptionsList(options);
  }, [options]);

  return (
    <div>
      <div className={style.top}>
        <button
          type='button'
          aria-haspopup='listbox'
          aria-expanded={isOpen}
          className={isOpen ? 'expanded' : ''}
          disabled={disabled}
          onClick={() => setOpen(!isOpen)}
        >
          {selectedOption == -1 ? placeholder : optionsList[selectedOption].label}
        </button>
        <button
          type='button'
          onClick={() => selectOption()}
          disabled={disabled}
        >
          Clear
        </button>
      </div>

      {
        isOpen ?
          (
            <ul
              className={style[`options-body${isOpen ? '' : '--hiden'}`]}
              role='listbox'
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
                      onDelete={onDeleteOption? deleteOption: undefined}
                    />
                  )
                )
              }
            </ul>
          ) :
          (<div></div>)
      }

    </div>
  );
}