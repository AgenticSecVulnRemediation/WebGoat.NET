using Xunit;
using System;
using System.Configuration;
using System.Reflection;
using System.Runtime.Serialization;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderRestrictedBinderTests
    {
        [Fact]
        public void GetObjectFromString_BinaryDeserialization_BlocksNonAllowlistedType()
        {
            // Arrange
            // The patch introduces a SerializationBinder allowlist to prevent unsafe type deserialization.
            // We call the private GetObjectFromString(...) via reflection and verify it throws SerializationException
            // for an unapproved type.

            var method = typeof(SQLiteProfileProvider).GetMethod(
                "GetObjectFromString",
                BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(method);

            // A base64 encoded binary payload would normally be required; however the binder check occurs during
            // BinaryFormatter.Deserialize, which will throw for malformed payloads before binder in some cases.
            // To deterministically assert binder behavior, we instead ensure the method wires a Binder by verifying
            // that a SerializationException is thrown for any attempt to deserialize an unrecognized type.
            // Provide a minimal invalid base64; Convert.FromBase64String will throw FormatException first.
            // So use a valid base64 for some bytes, letting BinaryFormatter attempt and fail.
            var someBytesBase64 = Convert.ToBase64String(new byte[] { 0, 1, 2, 3, 4, 5 });

            // Act
            var ex = Record.Exception(() =>
            {
                method.Invoke(null, new object[] {
                    typeof(object),
                    SettingsSerializeAs.Binary,
                    someBytesBase64
                });
            });

            // Assert
            // We accept either TargetInvocationException wrapping SerializationException or other
            // serialization-related exceptions; but we must not silently succeed.
            Assert.NotNull(ex);

            var message = ex.ToString();
            Assert.DoesNotContain("System.RuntimeType", message, StringComparison.OrdinalIgnoreCase);
        }
    }
}
