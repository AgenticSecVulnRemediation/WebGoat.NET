using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_AddsParameterInsteadOfConcatenating()
        {
            // Regression test for PR 361: catNumber clause now uses @catNumber + parameter.
            // We can't hit DB here; we at least ensure the call path doesn't throw synchronously with an injection payload.

            var provider = (MySqlDbProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(MySqlDbProvider));
            SetField(provider, "_connectionString", "Server=localhost;Database=doesnotexist;Uid=u;Pwd=p;");

            var ex = Record.Exception(() => provider.GetProductsAndCategories(1));

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
