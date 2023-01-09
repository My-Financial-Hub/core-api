/* eslint-disable @typescript-eslint/no-unused-vars */
declare interface Date {
  toISODateString: () => string;
}

Date.prototype.toISODateString = function(): string{
  return this.toISOString().split('T')[0];
};