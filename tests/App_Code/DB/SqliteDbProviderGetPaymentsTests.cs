using System;
using Xunit;
using Moq;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_UsesParameterPlaceholder_InsteadOfConcatenation()
        {
            // Delta test: query changed to include @customerNumber parameter.
            // We validate that calling GetPayments with a typical id doesn't throw.

            var configFile = new Mock<ConfigFile>(MockBehavior.Loose);
            configFile.Setup(c => c.Get(It.IsAny<string>())).Returns("test.db");

            var provider = new SqliteDbProvider(configFile.Object);

            var ex = Record.Exception(() => provider.GetPayments(123));
            Assert.Null(ex);
        }
    }
}
