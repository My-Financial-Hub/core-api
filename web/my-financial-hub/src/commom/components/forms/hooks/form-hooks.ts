import { ChangeEvent, useState } from 'react';

/**
* @obsolete needs useState<T>
**/
export function UseForm(onSubmit?: () => void) {
  const [submitItem, setItem] = useState<any>();

  const updateField = function(event: ChangeEvent<HTMLInputElement>){
    setItem({
      ...submitItem,
      [event.target.name]: event.target.value
    });
  };
  
  
  return {
    updateField,
    onSubmit
  };
}