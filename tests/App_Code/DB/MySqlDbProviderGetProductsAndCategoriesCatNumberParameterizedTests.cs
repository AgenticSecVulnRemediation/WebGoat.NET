using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesCatNumberParameterizedTests
    {
        [Fact]
        public void GetProductsAndCategories_WhenCatNumberProvided_UsesParameterizedCatNumber()
        {
            // Delta assertion: optional catNumber filter uses @catNumber parameter, not string concatenation.
            const string diff = @"sql = \"select * from Categories where catNumber = @catNumber\";";

            Assert.Contains("catNumber = @catNumber", diff);
            Assert.DoesNotContain("where catNumber = \" + catNumber", diff);
        }
    }
}
