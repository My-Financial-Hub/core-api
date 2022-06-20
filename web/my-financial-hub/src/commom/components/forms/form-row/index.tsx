/**
 * @deprecated Change to a global css stylesheet
**/
export default function FormRow({ children }: { children?: JSX.Element | JSX.Element[] }) {
  return (
    <div className='row'>
      {children}
    </div>
  );
}