using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesSingleLikeParameter_AppendsWildcardInParameterValue()
        {
            // Arrange
            var fileText = EmbeddedSourceReader.Read("WebGoat/App_Code/DB/MySqlDbProvider.cs");

            // Assert: SQL uses @Name parameter for both LIKE expressions and wildcard is appended in AddWithValue
            Assert.Contains("where firstName like @Name or lastName like @Name", fileText);
            Assert.Contains("Parameters.AddWithValue(\"@Name\", name + \"%\")", fileText);

            // Previously vulnerable concatenation should not remain.
            Assert.DoesNotContain("firstName like '\" + name + \"%'", fileText);
            Assert.DoesNotContain("lastName like '\" + name + \"%'", fileText);
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
