// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Data.Entity.Relational.FunctionalTests;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class AsyncFromSqlQuerySqlServerTest : AsyncFromSqlQueryTestBase<NorthwindSprocQuerySqlServerFixture>
    {
        public AsyncFromSqlQuerySqlServerTest(NorthwindSprocQuerySqlServerFixture fixture)
            : base(fixture)
        {
        }

        protected override string OpenDelimeter
        {
            get { return "["; }
        }

        protected override string CloseDelimeter
        {
            get { return "]"; }
        }
    }
}
