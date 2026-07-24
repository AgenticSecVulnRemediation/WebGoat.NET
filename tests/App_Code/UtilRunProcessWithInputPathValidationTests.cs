using System;
using System.Security;
using Xunit;
using Moq;

using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class UtilRunProcessWithInputPathValidationTests
    {
        [Theory]
        [InlineData("../evil.txt")]
        [InlineData("..\\evil.txt")]
        public void RunProcessWithInput_WithTraversal_ThrowsSecurityException(string inputPath)
        {
            // Arrange
            // Use harmless command: we won't reach process execution because validation should throw after Start().
            // However, the method starts the process before validation; pick something that exists.
            var cmd = Environment.OSVersion.Platform == PlatformID.Win32NT ? "cmd.exe" : "/bin/sh";
            var args = Environment.OSVersion.Platform == PlatformID.Win32NT ? "/c exit 0" : "-c 'exit 0'";

            // Act + Assert
            Assert.Throws<SecurityException>(() => Util.RunProcessWithInput(cmd, args, inputPath));
        }

        [Fact]
        public void RunProcessWithInput_WithRootedPath_ThrowsSecurityException()
        {
            // Arrange
            var inputPath = Environment.OSVersion.Platform == PlatformID.Win32NT ? "C:\\temp\\evil.txt" : "/tmp/evil.txt";
            var cmd = Environment.OSVersion.Platform == PlatformID.Win32NT ? "cmd.exe" : "/bin/sh";
            var args = Environment.OSVersion.Platform == PlatformID.Win32NT ? "/c exit 0" : "-c 'exit 0'";

            // Act + Assert
            Assert.Throws<SecurityException>(() => Util.RunProcessWithInput(cmd, args, inputPath));
        }
    }
}
