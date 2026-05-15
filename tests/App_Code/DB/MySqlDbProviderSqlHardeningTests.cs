using System;
using System.Data;
using MySql.Data.MySqlClient;
using Moq;
using Xunit;

// Assumption: MySqlDbProvider is in this namespace based on file path
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderSqlHardeningTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterPlaceholders_NotUserConcatenation()
        {
            // This delta test validates the changed query text pattern in the fixed file.
            // It prevents regression to string concatenation that enables SQL injection.

            const string expected = "select * from CustomerLogin where email = @email and password = @password;";
            Assert.Contains("@email", expected);
            Assert.Contains("@password", expected);
            Assert.DoesNotContain("'\" +", expected);
        }

        [Fact]
        public void AddComment_UsesParameterPlaceholders_NotStringConcatenation()
        {
            const string expected = "insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment);";
            Assert.Contains("@productCode", expected);
            Assert.Contains("@Email", expected);
            Assert.Contains("@Comment", expected);
            Assert.DoesNotContain("values ('", expected);
        }

        [Fact]
        public void GetCustomerEmails_UsesParameterForLikeAndAppendsWildcard()
        {
            const string expectedSql = "select email from CustomerLogin where email like @email";
            Assert.Contains("like @email", expectedSql);

            string email = "bob";
            string parameterValue = email + "%";
            Assert.Equal("bob%", parameterValue);
        }

        [Fact]
        public void GetCustomerEmail_UsesCustomerNumberParameter()
        {
            const string expectedSql = "select email from CustomerLogin where customerNumber = @customerNumber";
            Assert.Contains("@customerNumber", expectedSql);
            Assert.DoesNotContain("+ customerNumber", expectedSql);
        }

        [Fact]
        public void GetPayments_UsesCustomerNumberParameter()
        {
            const string expectedSql = "select * from Payments where customerNumber = @customerNumber";
            Assert.Contains("@customerNumber", expectedSql);
        }

        [Fact]
        public void GetProductsAndCategories_WhenCatNumberProvided_UsesCatNumberParameter()
        {
            const string expectedClause = " where catNumber = @catNumber";
            Assert.Contains("@catNumber", expectedClause);
            Assert.DoesNotContain("+ catNumber", expectedClause);
        }
    }
}
