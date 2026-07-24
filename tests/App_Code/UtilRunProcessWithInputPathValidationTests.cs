using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class UtilRunProcessWithInputPathValidationTests
    {
        [Theory]
        [InlineData("../secret.txt")]
        [InlineData("..\\secret.txt")]
        [InlineData("C:\\Windows\\system.ini")]
        [InlineData("/etc/passwd")]
        public void RunProcessWithInput_WhenPathTraversalOrRooted_ThrowsSecurityException(string input)
        {
            // PR 4590: method now blocks path traversal / rooted paths before opening file.

            Assert.Throws<System.Security.SecurityException>(() =>
                Util.RunProcessWithInput("someCmd", "someArgs", input));
        }
    }
}
