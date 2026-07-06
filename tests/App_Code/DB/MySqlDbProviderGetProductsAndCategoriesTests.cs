using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WhenCategoryProvided_UsesParameterForCatNumber()
        {
            // Arrange
            var fileText = EmbeddedSourceReader.Read("WebGoat/App_Code/DB/MySqlDbProvider.cs");

            // Assert: patched behavior uses @catNumber instead of concatenation
            Assert.Contains("catClause = \" where catNumber = @catNumber\"", fileText);
            Assert.Contains("Parameters.AddWithValue(\"@catNumber\", catNumber)", fileText);

            // Previously vulnerable concatenation pattern should not remain in the method.
            Assert.DoesNotContain("catClause += \" where catNumber = \" + catNumber", fileText);
        }
    }

    internal static class EmbeddedSourceReader
    {
        public static string Read(string relativePath)
        {
            var candidates = new[] { AppContext.BaseDirectory, System.IO.Directory.GetCurrentDirectory() };
            foreach (var baseDir in candidates)
            {
                var path = System.IO.Path.GetFullPath(System.IO.Path.Combine(baseDir, "..", "..", "..", "..", relativePath));
                if (System.IO.File.Exists(path))
                    return System.IO.File.ReadAllText(path);

                path = System.IO.Path.GetFullPath(System.IO.Path.Combine(baseDir, "..", "..", "..", relativePath));
                if (System.IO.File.Exists(path))
                    return System.IO.File.ReadAllText(path);
            }
            throw new InvalidOperationException($"Could not locate source file: {relativePath}");
        }
    }
}
