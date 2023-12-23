// System namespaces
global using System;
global using System.Threading.Tasks;
global using System.Collections.Generic;

// 3rd party test namespaces
global using NUnit.Framework;
global using Moq;

// Domain builder tests namespaces
global using FinancialHub.Core.Domain.Tests.Builders.Models;

// Common Project namespaces
global using FinancialHub.Common.Results;
global using FinancialHub.Common.Results.Errors;

// Domain Project namespaces
global using FinancialHub.Core.Domain.Models;
global using FinancialHub.Core.Domain.Entities;

// Domain interfaces namespaces
global using FinancialHub.Core.Domain.Interfaces.Services;
global using FinancialHub.Core.Domain.Interfaces.Providers;

[assembly: Category("Unit")]