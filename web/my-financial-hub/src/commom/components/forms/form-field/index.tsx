export default function FormFieldLabel({title, name,direction , children}: {title:string,direction?: 'row' | 'column', name:string, children: JSX.Element }) {
  return (
    <div className={direction == 'row'? 'd-flex flex-row' : 'd-flex flex-column'}>
      <label htmlFor={name}>{title}</label>
      {children}
    </div>
  );
}