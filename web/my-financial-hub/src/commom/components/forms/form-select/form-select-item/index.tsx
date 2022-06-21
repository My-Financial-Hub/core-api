import SelectOption from '../types/select-option';

export default function FormSelectItem(
  {
    option, isSelected,
    onSelect,onDelete
  }:
  {
    option: SelectOption, isSelected: boolean,
    onSelect: (option: SelectOption) => void,onDelete: (id:string) => void
  }
) {

  return (
    <li
      role="option"
      aria-selected={isSelected}
    >
      <ul>
        <li onClick={() => onSelect(option)}>{option.name}</li>
        <li onClick={() => onDelete(option.id)}>delete</li>
      </ul>
    </li>
  );
}