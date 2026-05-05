using System;
using System.Reflection;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderVerifyApplicationTests
    {
        [Fact]
        public void VerifyApplication_InsertsApplicationRecord_UsesPositionalParameters_DoesNotThrow()
        {
            // Delta test: VerifyApplication changed to VALUES (?, ?, ?) with AddRange(SQLiteParameter[]).
            // This test ensures the method can execute against SQLite without requiring named parameters.

            // Arrange
            var provider = new SQLiteProfileProvider();
            var cs = "Data Source=file:appdb?mode=memory&cache=shared";

            SetStaticField(typeof(SQLiteProfileProvider), "_connectionString", cs);
            SetStaticField(typeof(SQLiteProfileProvider), "_applicationName", "/testapp");
            SetStaticField(typeof(SQLiteProfileProvider), "_membershipApplicationName", "/testapp");
            SetStaticField(typeof(SQLiteProfileProvider), "_applicationId", null);
            SetStaticField(typeof(SQLiteProfileProvider), "_membershipApplicationId", null);

            using (var cn = new Mono.Data.Sqlite.SqliteConnection(cs))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE aspnet_Applications (ApplicationId TEXT, ApplicationName TEXT, Description TEXT);";
                    cmd.ExecuteNonQuery();
                }
            }

            // Act
            var method = typeof(SQLiteProfileProvider).GetMethod("VerifyApplication", BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(method);
            var ex = Record.Exception(() => method!.Invoke(null, null));

            // Assert
            Assert.Null(ex);

            using (var cn = new Mono.Data.Sqlite.SqliteConnection(cs))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM aspnet_Applications;";
                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    Assert.True(count >= 1);
                }
            }
        }

        private static void SetStaticField(Type t, string fieldName, object value)
        {
            var f = t.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }
    }
}
