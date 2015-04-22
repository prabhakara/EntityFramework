// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Data.Entity.Relational.FunctionalTests.TestModels.NorthwindSproc
{
    public static class NorthwindSprocData
    {
        public static MostExpensiveProduct[] TenMostExpensiveProducts()
        {
            // "dbo"."Ten Most Expensive Products"
            return new MostExpensiveProduct[]
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
                },
                new MostExpensiveProduct
                {
                    TenMostExpensiveProducts = "Mishi Kobe Niku",
                    UnitPrice = 97.00m
                },
                new MostExpensiveProduct
                {
                    TenMostExpensiveProducts = "Sir Rodney's Marmalade",
                    UnitPrice = 81.00m
                },
                new MostExpensiveProduct
                {
                    TenMostExpensiveProducts = "Carnarvon Tigers",
                    UnitPrice = 62.50m
                },
                new MostExpensiveProduct
                {
                    TenMostExpensiveProducts = "Raclette Courdavault",
                    UnitPrice = 55.00m
                },
                new MostExpensiveProduct
                {
                    TenMostExpensiveProducts = "Manjimup Dried Apples",
                    UnitPrice = 53.00m
                },
                new MostExpensiveProduct
                {
                    TenMostExpensiveProducts = "Tarte au sucre",
                    UnitPrice = 49.30m
                },
                new MostExpensiveProduct
                {
                    TenMostExpensiveProducts = "Ipoh Coffee",
                    UnitPrice = 46.00m
                },
                new MostExpensiveProduct
                {
                    TenMostExpensiveProducts = "Rössle Sauerkraut",
                    UnitPrice = 45.60m
                }
            };
        }

        public static CustomerOrderHistory[] CustomerOrderHistory()
        {
            // "dbo"."CustOrderHist" @CustomerID = 'ALFKI'
            return new CustomerOrderHistory[]
            {
                new CustomerOrderHistory
                {
                    ProductName = "Aniseed Syrup",
                    Total = 6
                },
                new CustomerOrderHistory
                {
                    ProductName = "Chartreuse verte",
                    Total = 21
                },
                new CustomerOrderHistory
                {
                    ProductName = "Escargots de Bourgogne",
                    Total = 40
                },
                new CustomerOrderHistory
                {
                    ProductName = "Flotemysost",
                    Total = 20
                },
                new CustomerOrderHistory
                {
                    ProductName = "Grandma's Boysenberry Spread",
                    Total = 16
                },
                new CustomerOrderHistory
                {
                    ProductName = "Lakkalikööri",
                    Total = 15
                },
                new CustomerOrderHistory
                {
                    ProductName = "Original Frankfurter grüne Soße",
                    Total = 2
                },
                new CustomerOrderHistory
                {
                    ProductName = "Raclette Courdavault",
                    Total = 15
                },
                new CustomerOrderHistory
                {
                    ProductName = "Rössle Sauerkraut",
                    Total = 17
                },
                new CustomerOrderHistory
                {
                    ProductName = "Spegesild",
                    Total = 2
                },
                new CustomerOrderHistory
                {
                    ProductName = "Vegie-spread",
                    Total = 20
                }
            };
        }
    }
}
