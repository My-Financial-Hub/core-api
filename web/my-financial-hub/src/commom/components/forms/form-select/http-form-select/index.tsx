/* eslint-disable @typescript-eslint/no-explicit-any */
import { useEffect, useState } from 'react';
import FormSelect from '..';
import Api from '../../../../http/api';
import SelectOption from '../types/select-option';

interface IHttpFormSelectProps{
  api: Api<any>,
  value?: string,
  placeholder?: string,
  disabled: boolean,
  onChangeOption?: (selectedOption?: SelectOption) => void,
  onDeleteOption?: (selectedOption?: SelectOption) => void
}

export default function HttpFormSelect(
  {
    api,value,
    disabled, placeholder = '',
    onChangeOption,onDeleteOption
  }:
  IHttpFormSelectProps
) {
  const [options, setOptions] = useState<SelectOption[]>([]);
  const [isLoading, setLoading] = useState<boolean>(true);

  const getData = async function(): Promise<void> {
    setLoading(true);
    const response = await api.GetAllAsync();
    if(!response.hasError){
      setOptions(
        response.data.map(x => 
          ({
            label: x['name'],
            value: x['id']
          })
        )
      );
      setLoading(false);
    }
  };

  const deleteData = async function(option :SelectOption): Promise<void>{
    setLoading(true);
    const response = await api.DeleteAsync(option.value);
    if(response){
      setOptions(options.filter(x => x.value === option.value));
      onDeleteOption?.(option);
    }
    setLoading(false);
  };

  useEffect(
    () => {
      getData();
    }, 
    []
  );

  return (
    <FormSelect 
      options={options}
      value={value}
      disabled={disabled || isLoading}
      placeholder={placeholder}
      onChangeOption={onChangeOption}
      onDeleteOption={onDeleteOption?()=> deleteData : undefined}
    />
  );
}