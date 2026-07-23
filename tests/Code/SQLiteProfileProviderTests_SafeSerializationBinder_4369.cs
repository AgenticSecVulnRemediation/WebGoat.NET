using System;
using System.IO;
using System.Runtime.Serialization;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests_SafeSerializationBinder_4369
    {
        [Fact]
        public void SafeSerializationBinder_WhenTypeNotAllowlisted_ThrowsSerializationException()
        {
            // Patch change: BinaryFormatter now uses SafeSerializationBinder with an allowlist.
            var binderType = typeof(SQLiteProfileProvider).Assembly.GetType("TechInfoSystems.Data.SQLite.SafeSerializationBinder");
            Assert.NotNull(binderType);

            var binder = (SerializationBinder)Activator.CreateInstance(binderType!)!;

            Assert.Throws<SerializationException>(() => binder.BindToType("mscorlib", "System.String"));
        }
    }
}
