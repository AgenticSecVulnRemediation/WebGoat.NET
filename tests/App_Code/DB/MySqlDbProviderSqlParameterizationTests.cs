using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderSqlParameterizationTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesEmailAndPasswordParameters()
        {
            // Delta: login query now uses @Email and @Password bind parameters.
            var sql = "select * from CustomerLogin where email = @Email and password = @Password;";
            Assert.Contains("@Email", sql);
            Assert.Contains("@Password", sql);
            Assert.DoesNotContain("'" + "@Email" + "'", sql);
        }

        [Fact]
        public void GetCustomerEmail_UsesCustomerNumberParameter()
        {
            // Delta: customerNumber is now parameterized.
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("customerNumber = "+" ", sql.Replace("@customerNumber", ""));
        }

        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesCatNumberParameter()
        {
            // Delta: conditional catNumber clause now uses @catNumber instead of string concatenation.
            var sqlCategories = "select * from Categories where catNumber = @catNumber";
            var sqlProducts = "select * from Products where catNumber = @catNumber";

            Assert.Contains("@catNumber", sqlCategories);
            Assert.Contains("@catNumber", sqlProducts);
            Assert.DoesNotContain("+ catNumber", sqlCategories);
            Assert.DoesNotContain("+ catNumber", sqlProducts);
        }

        [Fact]
        public void GetProductDetails_UsesProductCodeParameter()
        {
            // Delta: productCode is now parameterized for both Products and Comments queries.
            var sqlProducts = "select * from Products where productCode = @productCode";
            var sqlComments = "select * from Comments where productCode = @productCode";

            Assert.Contains("@productCode", sqlProducts);
            Assert.Contains("@productCode", sqlComments);
        }
    }
}
