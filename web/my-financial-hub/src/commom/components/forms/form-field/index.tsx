interface IFormFieldLabel{
  name:string, 
  title:string,
  error?:string,
  direction?: 'row' | 'column', 
  children: JSX.Element 
}

export default function FormFieldLabel({title, name, direction, error, children}: IFormFieldLabel) {
  return (
    <div className={direction == 'row'? 'd-flex flex-row' : 'd-flex flex-column'}>
      <label htmlFor={name}>{title}</label>
      {children}
      { error && (<span>{error}</span>) }
    </div>
  );
}