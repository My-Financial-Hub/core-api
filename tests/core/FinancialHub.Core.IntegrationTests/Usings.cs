// System namespaces
global using System;
global using System.Net;
global using System.Linq;
global using System.Collections.Generic;
global using System.Threading.Tasks;

// 3rd party test namespaces
global using NUnit.Framework;

// Domain Asserts namespaces
global using FinancialHub.Core.Domain.Tests.Assertions.Models;

// Common namespaces
global using FinancialHub.Common.Entities;

// Common Responses namespaces
global using FinancialHub.Common.Responses.Success;
global using FinancialHub.Common.Responses.Errors;

// Common Builders namespaces
global using FinancialHub.Common.Tests.Builders.Models;
global using FinancialHub.Common.Tests.Builders.Entities;

// Domain namespaces
global using FinancialHub.Core.Domain.Models;
global using FinancialHub.Core.Domain.Entities;
global using FinancialHub.Core.Domain.DTOS.Accounts;

// Domain Builders namespaces
global using FinancialHub.Core.Domain.Tests.Builders.Models;
global using FinancialHub.Core.Domain.Tests.Builders.Entities;

// Integration Tests SetUp namespaces
global using FinancialHub.Core.IntegrationTests.Base;
global using FinancialHub.Core.IntegrationTests.Setup;
global using FinancialHub.Core.IntegrationTests.Extensions;

[assembly: Category("Integration")]