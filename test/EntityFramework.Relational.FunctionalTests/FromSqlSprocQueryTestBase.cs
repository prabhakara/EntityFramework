// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.Data.Entity.FunctionalTests;
using Microsoft.Data.Entity.FunctionalTests.TestModels.Northwind;
using Microsoft.Data.Entity.Relational.FunctionalTests.TestModels.NorthwindSproc;
using Xunit;

namespace Microsoft.Data.Entity.Relational.FunctionalTests
{
    public abstract class FromSqlSprocQueryTestBase<TFixture> : IClassFixture<TFixture>
        where TFixture : NorthwindQueryFixtureBase, new()
    {
        [Fact]
        public virtual void From_sql_queryable_stored_procedure()
        {
            using (var context = CreateContext())
            {
                var actual = context
                    .Set<MostExpensiveProduct>()
                    .FromSql(TenMostExpensiveProductsSproc)
                    .ToArray();

                Assert.Equal(10, actual.Length);
                Assert.True(
                    actual.Contains(
                        new MostExpensiveProduct
                        {
                            TenMostExpensiveProducts = "Côte de Blaye",
                            UnitPrice = 263.50m
                        }));
            }
        }

        [Fact]
        public virtual void From_sql_queryable_stored_procedure_with_parameter()
        {
            using (var context = CreateContext())
            {
                var actual = context
                    .Set<CustomerOrderHistory>()
                    .FromSql(CustomerOrderHistorySproc, CustomerOrderHistoryParameters)
                    .ToArray();

                Assert.Equal(11, actual.Length);
                Assert.True(
                    actual.Contains(
                        new CustomerOrderHistory
                        {
                            ProductName = "Aniseed Syrup",
                            Total = 6
                        }));
            }
        }

        [Fact]
        public virtual void From_sql_queryable_stored_procedure_composed()
        {
            using (var context = CreateContext())
            {
                var actual = context
                    .Set<MostExpensiveProduct>()
                    .FromSql(TenMostExpensiveProductsSproc)
                    .Where(mep => mep.TenMostExpensiveProducts.Contains("C"))
                    .OrderBy(mep => mep.UnitPrice)
                    .ToArray();

                Assert.Equal(
                    new MostExpensiveProduct[]
                    {
                        new MostExpensiveProduct
                        {
                            TenMostExpensiveProducts = "Ipoh Coffee",
                            UnitPrice = 46.00m
                        },
                        new MostExpensiveProduct
                        {
                            TenMostExpensiveProducts = "Raclette Courdavault",
                            UnitPrice = 55.00m
                        },
                        new MostExpensiveProduct
                        {
                            TenMostExpensiveProducts = "Carnarvon Tigers",
                            UnitPrice = 62.50m
                        },
                        new MostExpensiveProduct
                        {
                            TenMostExpensiveProducts = "Côte de Blaye",
                            UnitPrice = 263.50m
                        }
                    },
                    actual);
            }
        }

        [Fact]
        public virtual void From_sql_queryable_stored_procedure_with_parameter_composed()
        {
            using (var context = CreateContext())
            {
                var actual = context
                    .Set<CustomerOrderHistory>()
                    .FromSql(CustomerOrderHistorySproc, CustomerOrderHistoryParameters)
                    .Where(coh => coh.ProductName.Contains("C"))
                    .OrderBy(coh => coh.Total)
                    .ToArray();


                Assert.Equal(
                    new CustomerOrderHistory[]
                    {
                        new CustomerOrderHistory
                        {
                            ProductName = "Raclette Courdavault",
                            Total = 15
                        },
                        new CustomerOrderHistory
                        {
                            ProductName = "Chartreuse verte",
                            Total = 21
                        }
                    },
                    actual);
            }
        }

        [Fact]
        public virtual void From_sql_queryable_stored_procedure_take()
        {
            using (var context = CreateContext())
            {
                var actual = context
                    .Set<MostExpensiveProduct>()
                    .FromSql(TenMostExpensiveProductsSproc)
                    .OrderByDescending(mep => mep.UnitPrice)
                    .Take(2)
                    .ToArray();

                Assert.Equal(
                    new MostExpensiveProduct[]
                    {
                        new MostExpensiveProduct
                        {
                            TenMostExpensiveProducts = "Côte de Blaye",
                            UnitPrice = 263.50m
                        },
                        new MostExpensiveProduct
                        {
                            TenMostExpensiveProducts = "Thüringer Rostbratwurst",
                            UnitPrice = 123.79m
                        }
                    },
                    actual);
            }
        }

        [Fact]
        public virtual void From_sql_queryable_stored_procedure_min()
        {
            using (var context = CreateContext())
            {
                Assert.Equal(
                    45.60m,
                    context
                    .Set<MostExpensiveProduct>()
                    .FromSql(TenMostExpensiveProductsSproc)
                    .Min(mep => mep.UnitPrice));
            }
        }

        [Fact]
        public virtual void From_sql_queryable_stored_procedure_with_include_throws()
        {
            using (var context = CreateContext())
            {
                Assert.Equal(
                    Strings.StoredProcedureIncludeNotSupported,
                    Assert.Throws<InvalidOperationException>(
                        () =>
                            context.Set<Product>()
                                .FromSql("SelectStoredProcedure")
                                .Include(p => p.OrderDetails)
                                .ToArray()
                        ).Message);
            }
        }

        protected NorthwindContext CreateContext()
        {
            return Fixture.CreateContext();
        }

        protected FromSqlSprocQueryTestBase(TFixture fixture)
        {
            Fixture = fixture;
        }

        protected TFixture Fixture { get; }

        protected abstract string TenMostExpensiveProductsSproc { get; }

        protected abstract string CustomerOrderHistorySproc { get; }

        protected abstract object[] CustomerOrderHistoryParameters { get; }
    }
}
