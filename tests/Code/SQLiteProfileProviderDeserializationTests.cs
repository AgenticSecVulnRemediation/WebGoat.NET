using System;
using System.Runtime.Serialization;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeserializationTests
    {
        [Fact]
        public void SafeBinder_BindToType_BlocksUnlistedType()
        {
            // Arrange
            var binder = new SafeBinder();

            // Act + Assert
            Assert.Throws<SerializationException>(() => binder.BindToType("mscorlib", "System.String"));
        }
    }
}
