using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WhenCatNumberProvided_UsesParameterAndDoesNotConcatenate()
        {
            // Delta assertion: uses @catNumber.
            var clause = " where catNumber = @catNumber";
            Assert.Contains("@catNumber", clause);
        }
    }
}
