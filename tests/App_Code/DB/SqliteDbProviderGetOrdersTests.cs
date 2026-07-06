using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterBinding_ForCustomerNumber()
        {
            // Arrange
            var fileText = EmbeddedSourceReader.Read("WebGoat/App_Code/DB/SqliteDbProvider.cs");

            // Assert
            Assert.Contains("select * from Orders where customerNumber = @customerNumber", fileText);
            Assert.Contains("da.SelectCommand.Parameters.AddWithValue(\"@customerNumber\", customerID)", fileText);

            // Previously vulnerable concatenation should not remain.
            Assert.DoesNotContain("select * from Orders where customerNumber = \" + customerID", fileText);
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
