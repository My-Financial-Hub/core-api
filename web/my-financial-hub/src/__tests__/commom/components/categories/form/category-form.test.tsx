import { render } from '@testing-library/react';
import assert from 'assert';

describe('on render', () =>{
  describe('default' , () =>{
    it('should render all fields', ()=>{
      assert.fail('not implemented');
    });
  
    it('should render submit button with text "Create"',  () => {
      assert.fail('not implemented');
    });

    it('should render all fields empty',  () => {
      assert.fail('not implemented');
    });
  });

  describe('with category', () =>{
    it('should fill the fields with data' , () =>{
      assert.fail('not implemented');
    });

    it('should render submit button with text "Update"',  () => {
      assert.fail('not implemented');
    });
  });
});

describe('on submit', ()=>{
  describe('create', () => { 
   it('should call create method', () =>{
    assert.fail('not implemented');
   });
   it('should add the category', () =>{
    assert.fail('not implemented');
   });
  });

  describe('update', () => { 
    it('should call update method', () =>{
      assert.fail('not implemented');
     });
     it('should update the category', () =>{
      assert.fail('not implemented');
     });
  });

  it('should reset form', () =>{
    assert.fail('not implemented');
   });
});

describe('on loading', ()=>{
  it('should disable all fields', () =>{
    assert.fail('not implemented');
  });
  it('should disable the submit button', () =>{
    assert.fail('not implemented');
  });
});