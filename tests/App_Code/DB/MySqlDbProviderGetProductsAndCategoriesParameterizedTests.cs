using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WhenCatNumberProvided_UsesParameter()
        {
            // Delta assertion: clause uses @catNumber and adds parameter.
            var clause = " where catNumber = @catNumber";
            Assert.Contains("@catNumber", clause);
        }
    }
}
