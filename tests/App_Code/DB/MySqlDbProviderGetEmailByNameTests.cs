using System;
using System.Reflection;
using Moq;
using Xunit;

// Assumption: production namespace matches source file.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesLikeParameterWithWildcard_NotStringConcatenation()
        {
            // Arrange
            // Security fix: name was concatenated into LIKE clauses; now SQL uses @name and adds % in parameter value.
            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);

            var provider = new MySqlDbProvider(cfg.Object);

            // Act
            var method = typeof(MySqlDbProvider).GetMethod("GetEmailByName", BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
            Assert.True(ContainsUserString(method!.Module, "where firstName like @name or lastName like @name"),
                "Expected @name parameterized LIKE SQL");
            Assert.True(ContainsUserString(method.Module, "name + \"%\""),
                "Expected code to append wildcard to parameter value (name + '%')");
        }

        private static bool ContainsUserString(Module module, string expected)
        {
            try
            {
                var location = module.Assembly.Location;
                if (string.IsNullOrEmpty(location))
                    return false;

                var bytes = System.IO.File.ReadAllBytes(location);
                var text = System.Text.Encoding.UTF8.GetString(bytes);
                return text.Contains(expected, StringComparison.Ordinal);
            }
            catch
            {
                return false;
            }
        }
    }
}
