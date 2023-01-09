import { useState } from 'react';

import { Transaction, TransactionStatus } from '../../interfaces/transaction';

import ValidationError from '../../interfaces/validation-error';

interface ErrorResult{
  hasError: boolean,
  validate: (transaction: Transaction) => void,
  getErrorMessage: (field: string) => string | undefined
}

//TODO : improve ValidationError return 
export default function UseTransactionValidator(): ErrorResult{
  const [errors, setErrors] = useState<ValidationError[]>([]);

  const validate = function(transaction: Transaction){
    const tempErrors = [...errors];
    if(!transaction.type){
      tempErrors.push({
        field: 'type',
        message: 'the transaction needs a type'
      });
    }

    if(!transaction.accountId){
      tempErrors.push({
        field: 'accountId',
        message: 'the transaction needs an account'
      });
    }

    if(transaction.amount <= 0){
      tempErrors.push({
        field: 'amount',
        message: 'the transaction amount needs to be bigger than 0'
      });
    }
  
    if(!transaction.categoryId){
      tempErrors.push({
        field: 'categoryId',
        message: 'the transaction needs a category'
      });
    }
  
    if(!transaction.targetDate){
      tempErrors.push({
        field: 'targetDate',
        message: 'the transaction needs a target date'
      });
    }
  
    if(!transaction.finishDate && transaction.status == TransactionStatus.Committed){
      tempErrors.push({
        field: 'finishDate',
        message: 'the transaction needs a finish date'
      });
    }

    setErrors(tempErrors);
  };

  const getErrorMessage = function(field: string) : string | undefined{
    if(errors.length > 0){
      return errors.find(x => x.field == field)?.message;
    }else{
      return undefined;
    }
  };

  return { hasError: errors.length > 0 , getErrorMessage, validate };
}