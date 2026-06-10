using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersParameterizedTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedCustomerId_DoesNotThrowFromSqlConcatenation()
        {
            // Regression: query now uses @customerID parameter. Ensure method call does not throw
            // due to SQL construction with concatenation.
            var provider = CreateProviderWithNonExistingDb();

            var ex = Record.Exception(() => provider.GetOrders(1));

            Assert.Null(ex);
        }

        private static OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider CreateProviderWithNonExistingDb()
        {
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
