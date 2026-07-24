using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SafeSerializationBinderTests
    {
        [Fact]
        public void BindToType_WithDisallowedType_ThrowsSerializationException()
        {
            // Arrange
            var binder = new SafeSerializationBinder();

            // Act + Assert
            Assert.Throws<SerializationException>(() => binder.BindToType("mscorlib", "System.String"));
        }

        [Fact]
        public void BinaryFormatter_WithBinder_RejectsDisallowedTypeDuringDeserialize()
        {
            // Arrange
            // Serialize a string and then attempt to deserialize it using the restricted binder.
            // Even though string itself is safe, it is NOT in the allowlist per fix and should be rejected.
            var ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, "hello");
            ms.Position = 0;

            var bf = new BinaryFormatter { Binder = new SafeSerializationBinder() };

            // Act + Assert
            Assert.Throws<SerializationException>(() => bf.Deserialize(ms));
        }
    }
}
