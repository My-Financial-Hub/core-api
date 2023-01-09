import { renderHook } from '@testing-library/react-hooks';

import UseTransactionValidator from '../../../../commom/hooks/validators/transaction-validator-hooks';
import { CreateTransaction } from '../../../../__mocks__/types/transaction-builder';

//TODO: improve tests
describe(
  'on validate', () =>{
    describe(
      'with valid data', ()=>{
        it(
          'should return hasError false', ()=>{
            const { result } = renderHook(() => UseTransactionValidator());

            const transaction = CreateTransaction();
            result.current.validate(transaction);

            expect(result.current.hasError).toBe(false);
          } 
        );
      }
    );
    describe(
      'with invalid data', ()=>{
        it(
          'should return hasError true', ()=>{
            const { result } = renderHook(() => UseTransactionValidator());

            const transaction = CreateTransaction();
            result.current.validate(transaction);

            expect(result.current.hasError).toBe(false);
          } 
        );
      }
    );
  }
);