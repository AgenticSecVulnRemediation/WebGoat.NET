using System;
using System.Reflection;
using System.Web.UI.WebControls;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoS_RegexTimeoutTests
    {
        [Fact]
        public void BtnCreateClick_UsesRegexWithTimeout_ToPreventCatastrophicBacktracking()
        {
            // Arrange
            var page = new OWASP.WebGoat.NET.RegexDoS();

            // Populate controls expected by the code-behind.
            page.GetType().GetField("txtUsername", BindingFlags.Instance | BindingFlags.NonPublic)!
                .SetValue(page, new TextBox { Text = "(a+)+$" });
            page.GetType().GetField("txtPassword", BindingFlags.Instance | BindingFlags.NonPublic)!
                .SetValue(page, new TextBox { Text = new string('a', 10000) + "X" });
            page.GetType().GetField("lblError", BindingFlags.Instance | BindingFlags.NonPublic)!
                .SetValue(page, new Label());

            var method = page.GetType().GetMethod("btnCreate_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);

            // Act + Assert
            // If no timeout is used, this pattern/input can hang.
            // With timeout, RegexMatchTimeoutException should be thrown quickly.
            var ex = Assert.Throws<TargetInvocationException>(() => method!.Invoke(page, new object?[] { page, EventArgs.Empty }));
            Assert.IsType<RegexMatchTimeoutException>(ex.InnerException);
        }
    }
}
