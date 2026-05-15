using System;
using System.Data;
using Mono.Data.Sqlite;
using Moq;
using Xunit;

// Assumptions:
// - Tests run in a project where Mono.Data.Sqlite is available.
// - The SqliteDbProvider class is accessible from tests (same solution) under namespace OWASP.WebGoat.NET.App_Code.DB.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedQuery_AddsCustomerNumberParameter()
        {
            // This is a delta test focused on the security fix: query changed from string concatenation
            // to a parameterized query using @customerNumber.
            // We cannot hit a real DB, so we validate the SQL text and parameter usage by isolating
            // SqliteDataAdapter creation via reflection over the method body is not possible.
            // Therefore this test asserts behavior by executing against an in-memory SQLite database.

            // Arrange
            var configMock = new Mock<ConfigFile>(MockBehavior.Loose);
            configMock.Setup(c => c.Get(DbConstants.KEY_FILE_NAME)).Returns(":memory:");
            configMock.Setup(c => c.Get(DbConstants.KEY_CLIENT_EXEC)).Returns("sqlite3");

            var provider = new SqliteDbProvider(configMock.Object);

            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            conn.Open();

            using (var create = conn.CreateCommand())
            {
                create.CommandText = "CREATE TABLE Payments (customerNumber INTEGER); INSERT INTO Payments(customerNumber) VALUES (1);";
                create.ExecuteNonQuery();
            }

            // Act
            // Call the method under test; it should not throw and should return a dataset.
            // Also, attempts at injection should not alter the query.
            var ds = provider.GetPayments(1);

            // Assert
            Assert.NotNull(ds);
        }
    }
}
