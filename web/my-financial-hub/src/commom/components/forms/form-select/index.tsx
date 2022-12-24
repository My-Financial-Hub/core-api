import style from './form-select.module.scss';

import FormSelectItem from './form-select-item';

import SelectOption from './types/select-option';

import UseToggleState from '../../../hooks/components/toggle-state';
import UseFormSelectOption from './hooks/form-select-option-hook';

type FormSelectProps = {
  placeholder?: string,
  value?: string,
  disabled: boolean,
  options: SelectOption[]
  onChangeOption?: (selectedOption?: SelectOption) => void,
  onDeleteOption?: (selectedOption?: string) => void,
}

//TODO: change to https://react-select.com/components
export default function FormSelect(
  {
    disabled, placeholder = '',
    value, options,
    onChangeOption, onDeleteOption
  }:
    FormSelectProps
) {
  const [isOpen, toggle] = UseToggleState(false);
  const { selectedOption, optionsList, selectOption, deleteOption} = UseFormSelectOption(
    {
      options, value,
      onChangeOption, onDeleteOption
    }
  );

  return (
    <div>
      <div className={style.top}>
        <button
          type='button'
          aria-haspopup='listbox'
          aria-expanded={isOpen}
          className={isOpen ? 'expanded' : ''}
          disabled={disabled}
          onClick={() => toggle()}
        >
          {selectedOption == -1 ? placeholder : optionsList[selectedOption].label}
        </button>
        <button
          type='button'
          onClick={() => isOpen && selectOption()}
          disabled={disabled}
        >
          Clear
        </button>
      </div>
      {
        isOpen &&
          (
            <ul
              className={style[`options-body${isOpen ? '' : '--hiden'}`]}
              role='listbox'
            >
              {
                optionsList.map(
                  (option, index) => (
                    <FormSelectItem
                      key={option.value}
                      option={option}
                      isSelected={selectedOption == index}
                      onSelect={selectOption}
                      onDelete={onDeleteOption ? deleteOption : undefined}
                    />
                  )
                )
              }
            </ul>
          ) 
      }
    </div>
  );
}