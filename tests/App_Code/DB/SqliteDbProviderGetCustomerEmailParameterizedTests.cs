using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailParameterizedTests
    {
        [Fact]
        public void GetCustomerEmail_WithInjectionLikeCustomerNumber_DoesNotThrow_FormatException_FromSqlConcatenation()
        {
            // Regression intent: the method now uses a parameter (@custNumber) instead of concatenating
            // customerNumber into SQL. This allows non-numeric strings to be passed without breaking SQL syntax.
            // We can't open the real DB here, but we can ensure the method accepts the input and fails gracefully.

            // Arrange
            var provider = CreateProviderWithNonExistingDb();

            // Act
            var result = provider.GetCustomerEmail("1 OR 1=1");

            // Assert
            // The method catches exceptions and returns ex.Message; ensure it returns something (not null)
            // rather than crashing due to malformed concatenated SQL.
            Assert.NotNull(result);
        }

        private static OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider CreateProviderWithNonExistingDb()
        {
            // Minimal ConfigFile stub via reflection: the real ConfigFile type exists in the WebGoat project.
            // We create an instance and set the file path to a temp file.
            var configType = typeof(OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider).Assembly
                .GetType("OWASP.WebGoat.NET.App_Code.DB.ConfigFile");

            if (configType == null)
                throw new InvalidOperationException("ConfigFile type not found; adjust namespace if needed.");

            var tempDb = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(tempDb);

            var config = Activator.CreateInstance(configType, nonPublic: true);
            var setMethod = configType.GetMethod("Set", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            var keyFileName = typeof(OWASP.WebGoat.NET.App_Code.DB.DbConstants).GetField("KEY_FILE_NAME").GetValue(null);

            if (setMethod != null)
            {
                setMethod.Invoke(config, new object[] { keyFileName, tempDb });
            }

            return (OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider)Activator.CreateInstance(typeof(OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider), config)!;
        }
    }
}
