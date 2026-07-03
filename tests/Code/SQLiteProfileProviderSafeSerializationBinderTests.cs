using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;
using Moq;
using Xunit;

// Assumptions:
// - We validate the behavior changed by the patch: BinaryFormatter uses a SerializationBinder that allows
//   only types with full name starting with "TechInfoSystems.Data.SQLite".
// - We avoid constructing a full SettingsPropertyValue by directly exercising SafeSerializationBinder via reflection.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSafeSerializationBinderTests
    {
        [Fact]
        public void SafeSerializationBinder_BindToType_DisallowsTypesOutsideNamespace()
        {
            // Arrange
            var providerType = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider);
            var binderType = providerType.GetNestedType("SafeSerializationBinder", BindingFlags.NonPublic);
            Assert.NotNull(binderType);

            var binder = Activator.CreateInstance(binderType!);
            Assert.NotNull(binder);

            var bindToType = binderType!.GetMethod("BindToType", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(bindToType);

            // Act + Assert
            var ex = Assert.Throws<TargetInvocationException>(() =>
                bindToType!.Invoke(binder, new object?[] { "mscorlib", "System.String" }));

            Assert.IsType<SerializationException>(ex.InnerException);
            Assert.Contains("not allowed", ex.InnerException!.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void SafeSerializationBinder_BindToType_AllowsTypesInNamespace_PrefersAssemblyQualifiedLookup()
        {
            // Arrange
            var providerType = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider);
            var binderType = providerType.GetNestedType("SafeSerializationBinder", BindingFlags.NonPublic);
            Assert.NotNull(binderType);

            var binder = Activator.CreateInstance(binderType!);
            var bindToType = binderType!.GetMethod("BindToType", BindingFlags.Public | BindingFlags.Instance);

            // We don't necessarily have a real concrete type in that namespace available in the test assembly.
            // But BindToType's contract post-fix is: it should NOT throw the "not allowed" error for matching prefixes;
            // it may return null if Type.GetType can't resolve. We assert it doesn't throw SerializationException.

            Exception? ex = null;
            try
            {
                bindToType!.Invoke(binder, new object?[] { "Some.Assembly", "TechInfoSystems.Data.SQLite.SomeType" });
            }
            catch (TargetInvocationException tie)
            {
                ex = tie.InnerException;
            }

            Assert.False(ex is SerializationException, "Expected types in allowed namespace prefix to not be rejected by binder.");
        }
    }
}
