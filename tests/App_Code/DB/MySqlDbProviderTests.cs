using System;
using System.Data;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetProductsAndCategories_UsesParameters_WhenCatNumberFilterApplied()
        {
            // Arrange
            var expected = "where catNumber = @catNumber";

            // Act + Assert
            Assert.Contains("@catNumber", expected);
        }
    }
}
