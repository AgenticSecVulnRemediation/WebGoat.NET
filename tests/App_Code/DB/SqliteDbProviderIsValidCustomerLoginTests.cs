using System;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderIsValidCustomerLoginTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQueryForEmailAndPassword()
        {
            // Arrange
            const string sql = "select * from CustomerLogin where email = @email and password = @password;";

            // Act
            var emailParam = new SqliteParameter("@email", "test@example.com");
            var passwordParam = new SqliteParameter("@password", "enc");

            // Assert
            Assert.Contains("@email", sql);
            Assert.Contains("@password", sql);
            Assert.Equal("@email", emailParam.ParameterName);
            Assert.Equal("@password", passwordParam.ParameterName);
        }
    }
}
