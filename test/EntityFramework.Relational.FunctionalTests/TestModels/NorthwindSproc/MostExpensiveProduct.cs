// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Data.Entity.Relational.FunctionalTests.TestModels.NorthwindSproc
{
    public class MostExpensiveProduct
    {
        public string TenMostExpensiveProducts { get; set; }

        public decimal? UnitPrice { get; set; }

        protected bool Equals(MostExpensiveProduct other)
        {
            return string.Equals(TenMostExpensiveProducts, other.TenMostExpensiveProducts)
                && UnitPrice == other.UnitPrice;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType()
                   && Equals((MostExpensiveProduct)obj);
        }

        public override int GetHashCode()
        {
            return TenMostExpensiveProducts.GetHashCode();
        }

        public override string ToString()
        {
            return "MostExpensiveProduct " + TenMostExpensiveProducts;
        }
    }
}
