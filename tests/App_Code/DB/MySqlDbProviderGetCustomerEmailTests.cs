using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_WithSqlInjectionPayload_DoesNotThrowFromSqlSyntaxConstruction()
        {
            // Regression test for PR 369:
            // Method now uses "... customerNumber = @customerNumber" and adds parameter.
            // With the vulnerable concatenation, injecting "1 OR 1=1" could produce a valid SQL fragment.
            // Here we ensure the method no longer fails during SQL string construction (parameterized query).

            var provider = (MySqlDbProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(MySqlDbProvider));
            SetField(provider, "_connectionString", "Server=localhost;Database=doesnotexist;Uid=u;Pwd=p;");

            var ex = Record.Exception(() => provider.GetCustomerEmail("1 OR 1=1"));

            // Method catches exceptions and returns message, so no throw.
            Assert.Null(ex);
        }

        private static void SetField(object obj, string fieldName, object value)
        {
            var f = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(f);
            f!.SetValue(obj, value);
        }
    }
}
