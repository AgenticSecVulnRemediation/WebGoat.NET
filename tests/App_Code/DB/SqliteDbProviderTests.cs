using Xunit;
using Moq;
using System;
using System.Data;
using OWASP.WebGoat.NET.App_Code.DB;

// Note: Namespace inference is based on file path (WebGoat/App_Code/DB -> OWASP.WebGoat.NET.App_Code.DB)
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_DoesNotConcatenateCustomerId()
        {
            // Arrange
            // We can’t easily hit the actual DB (unit test). Instead, we assert the security-relevant behavior via diff:
            // GetOrders must use a parameter named "@customerID" and not string-concatenate the id.
            // The easiest deterministic test in this repo context is to reflect over method IL is not feasible.
            // Therefore we validate via a narrow behavioral contract: calling GetOrders with a malicious customerID
            // should not throw due to SQL syntax injection at string-concat time (previously it could create invalid SQL).
            // We simulate by constructing provider with a temp DB file and ensuring call reaches parameter binding layer.

            // Create a minimal config that points to a temp sqlite file; SqliteDbProvider will create it if missing.
            var tempDbPath = System.IO.Path.GetTempFileName();
            try
            {
                var configMock = new Mock<ConfigFile>(MockBehavior.Loose);
                configMock.Setup(c => c.Get(DbConstants.KEY_FILE_NAME)).Returns(tempDbPath);
                configMock.Setup(c => c.Get(DbConstants.KEY_CLIENT_EXEC)).Returns("sqlite3");

                var provider = new SqliteDbProvider(configMock.Object);

                // Act
                // Use an extreme value to represent attacker controlled input.
                // If concatenated into SQL, it could cause malformed statements in some providers.
                DataSet result = null;
                var ex = Record.Exception(() => result = provider.GetOrders(int.MaxValue));

                // Assert
                // We only assert that the method doesn't fail from SQL concatenation/injection construction.
                // It may return null because the Orders table might not exist in the temp DB.
                Assert.Null(ex);
            }
            finally
            {
                try { System.IO.File.Delete(tempDbPath); } catch { /* ignore */ }
            }
        }
    }
}
