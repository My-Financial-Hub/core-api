using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Enums;
using FinancialHub.Domain.Queries;
using FinancialHub.Domain.Tests.Builders.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialHub.Domain.Tests.Queries
{
    public class TransactionQueryTests
    {
        private readonly Random random;
        private TransactionEntityBuilder transactionBuilder;

        public TransactionQueryTests()
        {
            this.random = new Random();
        }

        [SetUp]
        public void SetUp()
        {
            this.transactionBuilder = new TransactionEntityBuilder();
        }

        [Test]
        public void Query_AlwaysFilterByActiveStatus()
        {
            var size = this.random.Next(10,100);
            var entities = this.transactionBuilder.Generate(size);

            var query = new TransactionQuery().Query();

            var expectedResult = entities.Where(x => x.IsActive);
            var result = entities.Where(query);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Query_OnlyStartDate_FiltersStartDateMonth()
        {
            var thisDate = DateTime.Now;
            var size = this.random.Next(10, 100);

            var entity = this.transactionBuilder
                .WithTargetDate(thisDate)
                .WithActiveStatus(true)
                .Generate();
            var entities = this.transactionBuilder
                .WithTargetDateNextTo(thisDate,30)
                .Generate(size);
            entities.Add(entity);

            var transactionQuery = new TransactionQuery()
            {
                StartDate = thisDate
            };

            var result = entities.Where(transactionQuery.Query());

            foreach (var res in result)
            {
                Assert.AreEqual(thisDate.Month, res.TargetDate.Month);
                Assert.IsTrue(res.IsActive);
            }
            Assert.Greater(result.Count(), 0);
        }

        [Test]
        public void Query_OnlyEndDate_DoNotFilterByDate()
        {
            var endDate = DateTime.Now.AddMonths(-2);

            var size = this.random.Next(10, 100);
            var entities = this.transactionBuilder
                .WithTargetDateNextTo(endDate)
                .Generate(size);

            var transactionQuery = new TransactionQuery()
            {
                EndDate = endDate
            };

            var result = entities.Where(transactionQuery.Query());

            var expectedResult = entities.Where(x => x.IsActive);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Query_StartDateAndEndDate_FiltersBetweenDates()
        {
            var startDate = DateTime.Now.AddMonths(-2);
            var endDate = startDate.AddMonths(2);

            var size = this.random.Next(10, 100);
            var entities = this.transactionBuilder
                .WithTargetDateNextTo(startDate)
                .Generate(size);

            var transactionQuery = new TransactionQuery()
            {
                StartDate = startDate,
                EndDate = endDate
            };

            var result = entities.Where(transactionQuery.Query());

            var expectedResult = entities.Where(x =>
                x.IsActive &&
                x.TargetDate >= startDate &&
                x.TargetDate <= endDate
            );

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Query_Balances_FiltersByBalance()
        {
            var size = this.random.Next(10, 100);
            var entities = this.transactionBuilder.Generate(size);

            var balances = entities.Where(x=> x.Amount > 1000).Select(x => x.BalanceId).ToArray();

            var transactionQuery = new TransactionQuery()
            {
                Balances = balances
            };

            var result = entities.Where(transactionQuery.Query());

            var expectedResult = entities.Where(
                x => x.IsActive && balances.Contains(x.BalanceId)
            );

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Query_Categories_FiltersByCategory()
        {
            var size = this.random.Next(10, 100);
            var entities = this.transactionBuilder.Generate(size);

            var categories = entities.Where(x => x.Amount > 1000).Select(x => x.CategoryId).ToArray();
            var transactionQuery = new TransactionQuery()
            {
                Categories = categories
            };

            var result = entities.Where(transactionQuery.Query());

            var expectedResult = entities.Where(
                x => x.IsActive && categories.Contains(x.CategoryId)
            );

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Query_Types_FiltersByType()
        {
            var size = this.random.Next(10, 100);
            var entities = this.transactionBuilder.Generate(size);

            var types = new TransactionType[1]
            {
                TransactionType.Earn
            };
            var transactionQuery = new TransactionQuery()
            {
                Types = types
            };

            var result = entities.Where(transactionQuery.Query());

            var expectedResult = entities.Where(
                x => x.IsActive && types.Contains(x.Type)
            );

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Query_Status_FiltersByStatus()
        {
            var size = this.random.Next(10, 100);
            var entities = this.transactionBuilder.Generate(size);

            var status = new TransactionStatus[1]
            {
                TransactionStatus.Committed
            };
            var transactionQuery = new TransactionQuery()
            {
                Status = status
            };

            var result = entities.Where(transactionQuery.Query());

            var expectedResult = entities.Where(
                x => x.IsActive && status.Contains(x.Status)
            );

            Assert.AreEqual(expectedResult, result);
        }
    }
}
