using System;
using System.Reflection;
using System.Web.UI.WebControls;
using Xunit;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void BtnCreate_Click_WithCatastrophicBacktrackingPattern_ThrowsRegexMatchTimeoutException()
        {
            // Delta test: Regex constructed with explicit timeout.

            // Arrange
            var page = new RegexDoS();
            SetField(page, "txtUsername", new TextBox { Text = "^(a+)+$" });
            SetField(page, "txtPassword", new TextBox { Text = new string('a', 50000) + "!" });
            SetField(page, "lblError", new Label());

            var method = typeof(RegexDoS).GetMethod("btnCreate_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);

            // Act + Assert
            Assert.Throws<TargetInvocationException>(() => method!.Invoke(page, new object[] { null!, EventArgs.Empty }));

            // Unwrap and assert the inner exception is timeout.
            try
            {
                method!.Invoke(page, new object[] { null!, EventArgs.Empty });
            }
            catch (TargetInvocationException ex)
            {
                Assert.IsType<System.Text.RegularExpressions.RegexMatchTimeoutException>(ex.InnerException);
            }
        }

        private static void SetField(object instance, string fieldName, object value)
        {
            var f = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(f);
            f!.SetValue(instance, value);
        }
    }
}
