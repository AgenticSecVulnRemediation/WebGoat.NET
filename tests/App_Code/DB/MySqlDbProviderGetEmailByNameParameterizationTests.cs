using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameParameterizationTests
    {
        [Fact]
        public void GetEmailByName_UsesSingleNamedParameter_ForPrefixMatching()
        {
            // Arrange
            // Delta test for PR 4701: string concatenation replaced with @NamePrefix parameter.
            var sourcePath = System.IO.Path.Combine("WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");

            // Act
            var src = System.IO.File.ReadAllText(sourcePath);

            // Assert
            Assert.Contains("where firstName like @NamePrefix or lastName like @NamePrefix", src);
            Assert.Contains("cmd.Parameters.AddWithValue(\"@NamePrefix\"", src);

            // Previously vulnerable concatenation
            Assert.DoesNotContain("firstName like '\" + name", src);
            Assert.DoesNotContain("lastName like '\" + name", src);
        }
    }
}
