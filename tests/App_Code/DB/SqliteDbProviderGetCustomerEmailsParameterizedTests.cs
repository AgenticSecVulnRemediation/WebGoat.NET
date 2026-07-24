using System;
using Xunit;

// Assumption: SqliteDbProvider exists in OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsParameterizedTests
    {
        [Fact]
        public void GetCustomerEmails_WhenCalled_AcceptsPlainEmailPrefix_StringParameter()
        {
            // Arrange/Act
            var mi = typeof(SqliteDbProvider).GetMethod("GetCustomerEmails");

            // Assert
            Assert.NotNull(mi);
            Assert.Equal(typeof(string), mi!.GetParameters()[0].ParameterType);
        }
    }
}
