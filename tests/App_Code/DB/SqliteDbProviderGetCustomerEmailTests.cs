using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterBinding_InsteadOfConcatenation()
        {
            // Arrange
            var fileText = EmbeddedSourceReader.Read("WebGoat/App_Code/DB/SqliteDbProvider.cs");

            // Assert: fixed query uses @customerNumber and binds it.
            Assert.Contains("select email from CustomerLogin where customerNumber = @customerNumber", fileText);
            Assert.Contains("command.Parameters.AddWithValue(\"@customerNumber\", customerNumber)", fileText);

            // Previously vulnerable concatenation pattern should not remain for this query.
            Assert.DoesNotContain("select email from CustomerLogin where customerNumber = \" + customerNumber", fileText);
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
