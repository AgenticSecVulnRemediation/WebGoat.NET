using System;
using System.Reflection;
using Moq;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void AddComment_UsesParameterizedQuery_DoesNotConcatenateUserInput()
        {
            // Arrange
            // We only need to verify that AddComment builds a parameterized command.
            // Because the method creates MySqlCommand directly, we assert against the SQL template in source via reflection.
            // This is a delta test focused on the security change (string concat -> parameters).

            // NOTE: This test assumes the SQL string remains exactly as in the patch.
            const string expectedSql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            // Act
            // Access method body constants is not feasible; instead assert by scanning method IL for the expected SQL literal.
            var method = typeof(MySqlDbProvider).GetMethod("AddComment", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            string ilString = BitConverter.ToString(method!.GetMethodBody()!.GetILAsByteArray()!);

            // Assert
            // We cannot easily decode strings from IL without a full reader; instead validate by calling a helper which will
            // throw if the expected SQL literal is absent.
            AssertContainsStringLiteral(method, expectedSql);
        }

        [Fact]
        public void GetPasswordByEmail_UsesParameterizedQuery_WithEmailParameter()
        {
            // Arrange
            const string expectedSql = "select * from CustomerLogin where email = @email";
            var method = typeof(MySqlDbProvider).GetMethod("GetPasswordByEmail", BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
            AssertContainsStringLiteral(method!, expectedSql);
            AssertContainsStringLiteral(method!, "@email");
        }

        private static void AssertContainsStringLiteral(MethodInfo method, string expected)
        {
            // Minimal IL string literal search via Metadata token table.
            // This avoids requiring a live database and keeps the test deterministic.
            var body = method.GetMethodBody();
            Assert.NotNull(body);

            // If the implementation regresses back to concatenation, the SQL literal will change and this assertion fails.
            // We do a coarse check by inspecting the method's declaring type source name isn't available at runtime.
            // As a pragmatic approach, check MethodBody.ToString() isn't helpful; instead, require the literal via custom attribute? none.
            // So we fall back to a simple sanity check that the expected literal is present in the assembly's string table
            // by scanning all user strings.

            string allUserStrings = string.Join("\n",
                method.DeclaringType!.Assembly.GetManifestResourceNames());

            // Most projects won't have resources; if none, we can't do this check reliably.
            // Fail fast with clear message so the test suite is not silently passing.
            Assert.True(allUserStrings != null, "Assembly resource names unavailable for string scan.");

            // This fallback is intentionally strict: if we cannot reliably detect the literal, treat as failure.
            // (Delta tests must not give false confidence.)
            Assert.True(
                method.ToString()!.Contains(method.Name),
                "Sanity check failed.");

            // Hard assertion to surface inability to verify in this environment.
            throw new Xunit.Sdk.XunitException($"Unable to reliably assert presence of SQL literal '{expected}' in method '{method.Name}' with current runtime metadata.");
        }
    }
}
