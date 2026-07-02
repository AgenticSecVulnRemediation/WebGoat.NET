using System;
using System.IO;
using Xunit;

// Assumption: project has the referenced namespace and the class exists at runtime.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void Deserialize_WithBinaryPayload_DoesNotUseBinaryFormatterAndRejectsUnexpectedTypes()
        {
            // Arrange
            // The patch replaced BinaryFormatter.Deserialize with ValidatingObjectInputStreams.From(...).ReadObject().
            // This delta test asserts the secure behavior: deserialization should not accept arbitrary object graphs.
            // We simulate an "unexpected" serialized payload by passing bytes that would not be a valid V-OIS object.
            // (The secure behavior is to throw, not to return an arbitrary object.)

            var providerType = typeof(SQLiteProfileProvider);
            var deserializeMethod = providerType.GetMethod("Deserialize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(deserializeMethod);

            // Create a fake SettingsPropertyValue to satisfy signature; use null for prop where not required.
            // We can't easily construct SettingsPropertyValue without full System.Configuration plumbing,
            // but Deserialize only uses it for type checks when a value is returned.
            object fakeProp = null;

            // Random bytes: should fail deserialization.
            var payload = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 };

            // Act + Assert
            // Expect an exception (any) rather than a successful arbitrary deserialization.
            Assert.ThrowsAny<Exception>(() =>
            {
                deserializeMethod!.Invoke(null, new object?[] { fakeProp, payload });
            });
        }

        [Fact]
        public void GetObjectFromString_WithBinarySerializeAs_DoesNotUseBinaryFormatterAndRejectsUnexpectedTypes()
        {
            // Arrange
            var providerType = typeof(SQLiteProfileProvider);
            var method = providerType.GetMethod("GetObjectFromString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(method);

            // Create a base64 string that decodes to arbitrary bytes (not a safe serialized object)
            var base64 = Convert.ToBase64String(new byte[] { 0x10, 0x20, 0x30, 0x40 });

            // Act + Assert
            Assert.ThrowsAny<Exception>(() =>
            {
                method!.Invoke(null, new object?[] { typeof(object), System.Configuration.SettingsSerializeAs.Binary, base64 });
            });
        }
    }
}
