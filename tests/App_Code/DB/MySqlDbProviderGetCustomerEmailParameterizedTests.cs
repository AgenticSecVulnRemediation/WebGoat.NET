using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailParameterizedTests
    {
        [Fact]
        public void GetCustomerEmail_UsesCustomerNumberParameter()
        {
            // Delta: query changed to parameterized @customerNumber.
            var source = SourceText.Read("WebGoat/App_Code/DB/MySqlDbProvider.cs");
            Assert.Contains("where customerNumber = @customerNumber", source);
            Assert.Contains("Parameters.AddWithValue(\"@customerNumber\"", source);
            Assert.DoesNotContain("where customerNumber = \" + customerNumber", source);
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
