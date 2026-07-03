using System;
using System.Reflection;
using System.Runtime.Serialization;
using Xunit;

// Assumption: source namespace is TechInfoSystems.Data.SQLite as declared in SQLiteProfileProvider.cs
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderKnownTypesBinderTests
    {
        [Fact]
        public void KnownTypesBinder_BindToType_WithEmptyAllowList_ThrowsSerializationException()
        {
            // Arrange
            var providerType = typeof(SQLiteProfileProvider);
            // Nested type is private: TechInfoSystems.Data.SQLite.SQLiteProfileProvider+KnownTypesBinder
            var binderType = providerType.GetNestedType("KnownTypesBinder", BindingFlags.NonPublic);
            Assert.NotNull(binderType);

            var binder = (SerializationBinder)Activator.CreateInstance(binderType!, nonPublic: true)!;

            // Act + Assert
            var ex = Assert.Throws<SerializationException>(() => binder.BindToType("any", "System.String"));
            Assert.Contains("is not allowed", ex.Message);
        }
    }
}
