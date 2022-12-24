import { renderHook } from '@testing-library/react-hooks';

import UseTransactionValidator from '../../../../commom/hooks/validators/transaction-validator-hooks';

//TODO: improve tests
describe(
  'on validate', () =>{
    describe(
      'with valid data', ()=>{
        it(
          'should return hasError false', ()=>{
            const { result } = renderHook(() => UseTransactionValidator());

            expect(true).toBe(false);
          } 
        );
        it(
          'should not return any error messages', ()=>{
            const { result } = renderHook(() => UseTransactionValidator());

            expect(true).toBe(false);
          } 
        );
      }
    );
    describe(
      'with invalid data', ()=>{
        it(
          'should return hasError true', ()=>{
            const { result } = renderHook(() => UseTransactionValidator());

            expect(true).toBe(false);
          } 
        );
      }
    );
  }
);