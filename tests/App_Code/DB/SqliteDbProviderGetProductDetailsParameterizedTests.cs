using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsParameterizedTests
    {
        [Fact]
        public void GetProductDetails_UsesProductCodeParameter_ForProductsAndComments()
        {
            // Delta: both Products and Comments queries changed to parameterized @productCode.
            var source = SourceText.Read("WebGoat/App_Code/DB/SqliteDbProvider.cs");
            Assert.Contains("Products where productCode = @productCode", source);
            Assert.Contains("Comments where productCode = @productCode", source);
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
