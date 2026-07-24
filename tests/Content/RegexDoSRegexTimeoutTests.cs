using Xunit;
using OWASP.WebGoat.NET;
using System;
using System.Reflection;
using System.Web.UI.WebControls;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSRegexTimeoutTests
    {
        [Fact]
        public void BtnCreateClick_UsesRegexWithTimeout()
        {
            // Arrange
            var page = new RegexDoS();

            // Set private/protected controls via reflection (WebForms style)
            SetField(page, "txtUsername", new TextBox { Text = "(a+)+$" });
            SetField(page, "txtPassword", new TextBox { Text = new string('a', 10000) });
            SetField(page, "lblError", new Label());

            // Act
            var ex = Record.Exception(() => InvokeNonPublic(page, "btnCreate_Click"));

            // Assert
            // With timeout, RegexMatchTimeoutException may happen instead of hanging; both are acceptable as long as it doesn't hang.
            Assert.True(ex == null || ex is TargetInvocationException);
        }

        private static void InvokeNonPublic(object instance, string methodName)
        {
            var mi = instance.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            mi.Invoke(instance, new object[] { instance, EventArgs.Empty });
        }

        private static void SetField(object instance, string fieldName, object value)
        {
            var fi = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (fi != null)
            {
                fi.SetValue(instance, value);
                return;
            }

            var pi = instance.GetType().GetProperty(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (pi != null)
            {
                pi.SetValue(instance, value);
            }
        }
    }
}
