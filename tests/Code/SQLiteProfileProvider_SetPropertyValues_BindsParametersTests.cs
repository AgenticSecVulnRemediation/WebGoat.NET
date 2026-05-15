using System;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

// Delta-focused test: SetPropertyValues now uses AddRange with SqliteParameter, ensuring both $Username and $ApplicationId are bound.
// We validate that it does not throw and that it creates a user and profile row for an anonymous user.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProvider_SetPropertyValues_BindsParametersTests
    {
        private static void SetStaticField(Type t, string name, object? value)
        {
            var f = t.GetField(name, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }

        private static SQLiteProfileProvider CreateProvider(string cs)
        {
            // bypass Initialize; set private statics
            var p = new SQLiteProfileProvider();
            SetStaticField(typeof(SQLiteProfileProvider), "_connectionString", cs);
            SetStaticField(typeof(SQLiteProfileProvider), "_membershipApplicationId", "app1");
            return p;
        }

        [Fact]
        public void SetPropertyValues_AnonymousUser_BindsUsernameAndApplicationId_AndSavesProfile()
        {
            // Arrange
            var cs = "Data Source=file:memdb4?mode=memory&cache=shared";
            using var keeper = new SqliteConnection(cs);
            keeper.Open();

            using (var cmd = keeper.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE [aspnet_Users] (
  UserId TEXT,
  Username TEXT,
  LoweredUsername TEXT,
  ApplicationId TEXT,
  Email TEXT,
  LoweredEmail TEXT,
  Comment TEXT,
  Password TEXT,
  PasswordFormat TEXT,
  PasswordSalt TEXT,
  PasswordQuestion TEXT,
  PasswordAnswer TEXT,
  IsApproved INTEGER,
  IsAnonymous INTEGER,
  CreateDate TEXT,
  LastPasswordChangedDate TEXT,
  LastActivityDate TEXT,
  LastLoginDate TEXT,
  IsLockedOut INTEGER,
  LastLockoutDate TEXT,
  FailedPasswordAttemptCount INTEGER,
  FailedPasswordAttemptWindowStart TEXT,
  FailedPasswordAnswerAttemptCount INTEGER,
  FailedPasswordAnswerAttemptWindowStart TEXT
);
CREATE TABLE [aspnet_Profile] (
  UserId TEXT,
  PropertyNames TEXT,
  PropertyValuesString TEXT,
  PropertyValuesBinary BLOB,
  LastUpdatedDate TEXT
);
";
                cmd.ExecuteNonQuery();
            }

            var provider = CreateProvider(cs);

            var ctx = new SettingsContext();
            ctx["UserName"] = "anon";
            ctx["IsAuthenticated"] = false;

            var props = new SettingsPropertyCollection();
            var sp = new SettingsProperty("P1")
            {
                PropertyType = typeof(string),
                SerializeAs = SettingsSerializeAs.String,
                Provider = provider,
                DefaultValue = "",
            };
            sp.Attributes["AllowAnonymous"] = true;
            props.Add(sp);

            var values = new SettingsPropertyValueCollection();
            var v = new SettingsPropertyValue(sp) { PropertyValue = "v1", IsDirty = true };
            values.Add(v);

            // Act
            var ex = Record.Exception(() => provider.SetPropertyValues(ctx, values));

            // Assert
            Assert.Null(ex);
            using (var verify = keeper.CreateCommand())
            {
                verify.CommandText = "SELECT COUNT(*) FROM [aspnet_Users]";
                Assert.Equal(1L, (long)verify.ExecuteScalar()!);
                verify.CommandText = "SELECT COUNT(*) FROM [aspnet_Profile]";
                Assert.Equal(1L, (long)verify.ExecuteScalar()!);
            }
        }
    }
}
