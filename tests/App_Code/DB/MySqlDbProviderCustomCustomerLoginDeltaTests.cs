// Assumptions:
// - Namespace is OWASP.WebGoat.NET.App_Code.DB
// - This delta test verifies the CustomCustomerLogin query was changed to use a parameter placeholder (@Email)
//   and that a parameter is added, preventing string concatenation.
// - Deterministic verification is done by scanning for key query fragments in the compiled assembly.

using System.Reflection;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomCustomerLoginDeltaTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesEmailParameterPlaceholder()
        {
            var asm = typeof(OWASP.WebGoat.NET.App_Code.DB.MySqlDbProvider).Assembly;
            var allStrings = GetAllUserStrings(asm);

            Assert.Contains("select * from CustomerLogin where email = @Email;", allStrings);
            Assert.Contains("cmd.Parameters.AddWithValue(\"@Email\"", allStrings);
        }

        private static string GetAllUserStrings(Assembly asm)
        {
            var location = asm.Location;
            if (string.IsNullOrWhiteSpace(location) || !System.IO.File.Exists(location))
            {
                return string.Empty;
            }

            var bytes = System.IO.File.ReadAllBytes(location);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}
