// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Data.Entity.Relational.FunctionalTests.TestModels.NorthwindSproc
{
    public class CustomerOrderHistory
    {
        public string ProductName { get; set; }

        public int Total { get; set; }

        protected bool Equals(CustomerOrderHistory other)
        {
            return string.Equals(ProductName, other.ProductName)
                && Total == other.Total;
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
                   && Equals((CustomerOrderHistory)obj);
        }

        public override int GetHashCode()
        {
            return ProductName.GetHashCode();
        }

        public override string ToString()
        {
            return "CustomerOrderHistory " + ProductName;
        }
    }
}
