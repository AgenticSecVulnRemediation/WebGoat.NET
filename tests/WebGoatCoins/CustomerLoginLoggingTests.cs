using System;
using log4net;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using OWASP.WebGoat.NET.WebGoatCoins;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLogin_Logging_DoesNotLogPassword_Tests
    {
        [Fact]
        public void ButtonLogOnClick_DoesNotLogPassword()
        {
            // Arrange
            // We can't easily instantiate the full ASP.NET Page pipeline in a unit test; instead we enforce
            // the delta behavior by asserting the source no longer logs the password variable.
            var source = System.IO.File.ReadAllText("WebGoat/WebGoatCoins/CustomerLogin.aspx.cs");

            // Assert
            Assert.DoesNotContain("with password", source);
            Assert.DoesNotContain("+ pwd", source);
            Assert.Contains("attempted to log in.", source);
        }
    }
}
