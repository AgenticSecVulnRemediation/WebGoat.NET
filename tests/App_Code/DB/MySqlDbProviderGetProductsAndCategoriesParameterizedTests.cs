using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesParameterizedTests
    {
        [Fact]
        public void GetProductsAndCategories_WhenCatNumberProvided_UsesCatNumberParameter()
        {
            // Delta: catClause changed to use @catNumber and parameters are added on SelectCommand.
            var source = SourceText.Read("WebGoat/App_Code/DB/MySqlDbProvider.cs");
            Assert.Contains("where catNumber = @catNumber", source);
            Assert.Contains("Parameters.AddWithValue(\"@catNumber\"", source);
            Assert.DoesNotContain("where catNumber = \" + catNumber", source);
        }
    }

    internal static class SourceText
    {
        public static string Read(string resourcePath)
        {
            var asm = typeof(SourceText).Assembly;
            var normalized = resourcePath.Replace('/', '.').Replace('\\', '.');
            foreach (var name in asm.GetManifestResourceNames())
            {
                if (name.EndsWith(normalized, StringComparison.OrdinalIgnoreCase))
                {
                    using var s = asm.GetManifestResourceStream(name);
                    using var r = new System.IO.StreamReader(s!);
                    return r.ReadToEnd();
                }
            }
            throw new InvalidOperationException($"Embedded resource not found for '{resourcePath}'. Ensure the source file is embedded for this delta test.");
        }
    }
}
