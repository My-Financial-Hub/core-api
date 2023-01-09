import { useState } from 'react';

type ToggleState = [boolean, ()=> void]
export default function UseToggleState(defaultValue = true): ToggleState{
  const [ isEnabled, setEnabled] = useState<boolean>(defaultValue);

  const toggle = function(): void{
    setEnabled(!isEnabled);
  };

  return [isEnabled, toggle];
}