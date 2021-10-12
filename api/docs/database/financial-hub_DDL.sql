CREATE DATABASE financial_hub;
USE financial_hub;

CREATE TABLE accounts(
  id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
  name VARCHAR(200) NOT NULL,
  description VARCHAR(500) NULL,
  currency VARCHAR(50),
  active BIT DEFAULT 1,
  update_time DATETIMEOFFSET DEFAULT GETUTCDATE(),
  creation_time DATETIMEOFFSET DEFAULT GETUTCDATE(),
);