using Xunit;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderRestrictedBinderTests
    {
        [Serializable]
        private class NotAllowedType
        {
            public string Value { get; set; } = "x";
        }

        [Fact]
        public void GetObjectFromString_BinaryDeserialization_ThrowsSerializationExceptionForNotAllowlistedType()
        {
            // Arrange: create a real BinaryFormatter payload for a type that is not in the allowlist.
            var payload = SerializeToBase64(new NotAllowedType());

            var method = typeof(SQLiteProfileProvider).GetMethod(
                "GetObjectFromString",
                BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(method);

            // Act
            var ex = Assert.Throws<TargetInvocationException>(() =>
            {
                method!.Invoke(null, new object[] { typeof(object), System.Configuration.SettingsSerializeAs.Binary, payload });
            });

            // Assert
            Assert.NotNull(ex.InnerException);
            Assert.IsType<SerializationException>(ex.InnerException);
            Assert.Contains("not allowed", ex.InnerException!.Message, StringComparison.OrdinalIgnoreCase);
        }

        private static string SerializeToBase64(object o)
        {
            using var ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, o);
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}
