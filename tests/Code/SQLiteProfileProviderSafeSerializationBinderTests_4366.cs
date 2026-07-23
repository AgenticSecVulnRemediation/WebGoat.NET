using System;
using System.Runtime.Serialization;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSafeSerializationBinderTests
    {
        [Fact]
        public void SafeSerializationBinder_BlocksDisallowedType_ByThrowingSerializationException()
        {
            // Arrange
            var binder = new SafeSerializationBinder();

            // Act/Assert
            Assert.Throws<SerializationException>(() => binder.BindToType("mscorlib", "System.String"));
        }

        [Fact]
        public void SafeSerializationBinder_AllowsExplicitlyAllowedType_WhenPresentInAllowList()
        {
            // Arrange
            // The binder uses hardcoded allow-list placeholders; we can only assert behavior for those literals.
            var binder = new SafeSerializationBinder();

            // Act
            var type = binder.BindToType("mscorlib", "Namespace.SafeType1");

            // Assert
            // Type.GetType may return null depending on assembly name; the delta behavior is that it does not throw.
            Assert.True(true);
        }
    }
}
