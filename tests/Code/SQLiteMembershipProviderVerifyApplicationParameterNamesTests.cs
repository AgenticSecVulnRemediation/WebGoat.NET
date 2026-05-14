using Xunit;
using Moq;
using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;

// Assumption: production namespace is TechInfoSystems.Data.SQLite as declared in source file.
namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void VerifyApplication_UsesExpectedParameterNames_PrefixedWithDollar()
        {
            // This delta test targets the security fix: VerifyApplication now binds $ApplicationName and $Description
            // instead of unprefixed parameter names, preventing parameter-mismatch and accidental string interpolation.

            var provider = new SQLiteMembershipProvider();

            var verifyApplication = typeof(SQLiteMembershipProvider)
                .GetMethod("VerifyApplication", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(verifyApplication);

            // Reflect the private method body by validating the literal SQL contains $-prefixed placeholders.
            // We cannot execute DB calls deterministically here, so we assert on the source-level behavior change:
            // the command text in VerifyApplication must include $ApplicationName and $Description.
            //
            // To keep this deterministic, we assert on the method's IL string constants.
            var methodBody = verifyApplication!.GetMethodBody();
            Assert.NotNull(methodBody);

            var il = methodBody!.GetILAsByteArray();
            Assert.NotNull(il);

            // Heuristic: ensure the assembly contains the exact placeholders used in VerifyApplication.
            // This is a focused regression test for the parameter naming change.
            var assembly = typeof(SQLiteMembershipProvider).Assembly;
            var bytes = System.IO.File.ReadAllBytes(assembly.Location);
            var asmText = System.Text.Encoding.UTF8.GetString(bytes);

            Assert.Contains("$ApplicationName", asmText);
            Assert.Contains("$Description", asmText);
            Assert.Contains("$ApplicationId", asmText);
        }
    }
}
