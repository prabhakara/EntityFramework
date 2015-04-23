// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity.FunctionalTests;
using Microsoft.Data.Entity.FunctionalTests.TestModels.Northwind;
using Microsoft.Data.Entity.Tests;
using Xunit;

namespace Microsoft.Data.Entity.Relational.FunctionalTests
{
    public abstract class AsyncFromSqlQueryTestBase<TFixture> : IClassFixture<TFixture>
        where TFixture : NorthwindQueryFixtureBase, new()
    {
        [Fact]
        public virtual async Task From_sql_queryable_simple()
        {
            await AssertQuery<Customer>(
                cs => cs.FromSql("SELECT * FROM Customers"),
                cs => cs,
                entryCount: 91);
        }

        [Fact]
        public virtual async Task From_sql_queryable_filter()
        {
            await AssertQuery<Customer>(
                cs => cs.FromSql("SELECT * FROM Customers WHERE Customers.ContactName LIKE '%z%'"),
                cs => cs.Where(c => c.ContactName.Contains("z")),
                entryCount: 14);
        }

        [Fact]
        public virtual async Task From_sql_queryable_composed()
        {
            await AssertQuery<Customer>(
                cs => cs.FromSql("SELECT * FROM Customers").Where(c => c.ContactName.Contains("z")),
                cs => cs.Where(c => c.ContactName.Contains("z")),
                entryCount: 14);
        }

        [Fact]
        public virtual async Task From_sql_queryable_multiple_line_query()
        {
            await AssertQuery<Customer>(
                cs => cs.FromSql(@"SELECT *
FROM Customers
WHERE Customers.City = 'London'"),
                cs => cs.Where(c => c.City == "London"),
                entryCount: 6);
        }

        [Fact]
        public virtual async Task From_sql_queryable_composed_multiple_line_query()
        {
            await AssertQuery<Customer>(
                cs => cs.FromSql(@"SELECT *
FROM Customers").Where(c => c.City == "London"),
                cs => cs.Where(c => c.City == "London"),
                entryCount: 6);
        }

        [Fact]
        public virtual async Task From_sql_queryable_with_parameters()
        {
            var city = "London";
            var contactTitle = "Sales Representative";

            await AssertQuery<Customer>(
                cs => cs.FromSql(@"SELECT * FROM Customers WHERE City = {0} AND ContactTitle = {1}", city, contactTitle),
                cs => cs.Where(c => c.City == city && c.ContactTitle == contactTitle),
                entryCount: 3);
        }

        [Fact]
        public virtual async Task From_sql_queryable_with_parameters_and_closure()
        {
            var city = "London";
            var contactTitle = "Sales Representative";

            await AssertQuery<Customer>(
                cs => cs.FromSql(@"SELECT * FROM Customers WHERE City = {0}", city).Where(c => c.ContactTitle == contactTitle),
                cs => cs.Where(c => c.City == city && c.ContactTitle == contactTitle),
                entryCount: 3);
        }

        [Fact]
        public virtual async Task From_sql_queryable_simple_cache_key_includes_query_string()
        {
            await AssertQuery<Customer>(
                cs => cs.FromSql("SELECT * FROM Customers WHERE Customers.City = 'London'"),
                cs => cs.Where(c => c.City == "London"),
                entryCount: 6);

            await AssertQuery<Customer>(
                cs => cs.FromSql("SELECT * FROM Customers WHERE Customers.City = 'Seattle'"),
                cs => cs.Where(c => c.City == "Seattle"),
                entryCount: 1);
        }

        [Fact]
        public virtual async Task From_sql_queryable_with_parameters_cache_key_includes_parameters()
        {
            var city = "London";
            var contactTitle = "Sales Representative";
            var sql = @"SELECT * FROM Customers WHERE City = {0} AND ContactTitle = {1}";

            await AssertQuery<Customer>(
                cs => cs.FromSql(sql, city, contactTitle),
                cs => cs.Where(c => c.City == city && c.ContactTitle == contactTitle),
                entryCount: 3);

            city = "Madrid";
            contactTitle = "Accounting Manager";

            await AssertQuery<Customer>(
                cs => cs.FromSql(sql, city, contactTitle),
                cs => cs.Where(c => c.City == city && c.ContactTitle == contactTitle),
                entryCount: 2);
        }

        [Fact]
        public virtual async Task From_sql_queryable_simple_as_no_tracking_not_composed()
        {
            await AssertQuery<Customer>(
                cs => cs.FromSql("SELECT * FROM Customers").AsNoTracking(),
                cs => cs,
                entryCount: 0);
        }

        [Fact]
        public virtual async Task From_sql_queryable_simple_include()
        {
            await AssertQuery<Customer>(
                cs => cs.FromSql("SELECT * FROM Customers").Include(c => c.Orders),
                cs => cs,
                entryCount: 921);
        }

        [Fact]
        public virtual async Task From_sql_queryable_simple_composed_include()
        {
            await AssertQuery<Customer>(
                cs => cs.FromSql("SELECT * FROM Customers").Where(c => c.City == "London").Include(c => c.Orders),
                cs => cs.Where(c => c.City == "London"),
                entryCount: 52);
        }

        [Fact]
        public virtual async Task From_sql_annotations_do_not_affect_successive_calls()
        {
            using (var context = CreateContext())
            {
                TestHelpers.AssertResults(
                    NorthwindData.Set<Customer>().Where(c => c.ContactName.Contains("z")).ToArray(),
                    await context.Customers.FromSql("SELECT * FROM Customers WHERE Customers.ContactName LIKE '%z%'").ToArrayAsync(),
                    assertOrder: false);

                Assert.Equal(14, context.ChangeTracker.Entries().Count());

                TestHelpers.AssertResults(
                    NorthwindData.Set<Customer>().ToArray(),
                    await context.Customers.ToArrayAsync(),
                    assertOrder: false);

                Assert.Equal(91, context.ChangeTracker.Entries().Count());
            }
        }

        protected NorthwindContext CreateContext()
        {
            return Fixture.CreateContext();
        }

        protected AsyncFromSqlQueryTestBase(TFixture fixture)
        {
            Fixture = fixture;
        }

        protected TFixture Fixture { get; }

        private async Task AssertQuery<TItem>(
            Func<DbSet<TItem>, IQueryable<object>> relationalQuery,
            Func<IQueryable<TItem>, IQueryable<object>> l2oQuery,
            bool assertOrder = false,
            int entryCount = 0,
            Action<IList<object>, IList<object>> asserter = null)
            where TItem : class
        {
            using (var context = CreateContext())
            {
                TestHelpers.AssertResults(
                    l2oQuery(NorthwindData.Set<TItem>()).ToArray(),
                    await relationalQuery(context.Set<TItem>()).ToArrayAsync(),
                    assertOrder,
                    asserter);
                Assert.Equal(entryCount, context.ChangeTracker.Entries().Count());
            }
        }
    }
}
