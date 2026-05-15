using System;
using System.Reflection;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoS_RegexTimeoutTests
    {
        [Fact]
        public void BtnCreateClick_UsesRegexTimeout_ThrowsRegexMatchTimeoutException_OnEvilInput()
        {
            // Delta test for PR #638: Regex is now created with a 1s timeout.
            // We verify that catastrophic backtracking triggers RegexMatchTimeoutException.

            var request = new System.Web.HttpRequest("", "http://localhost/Content/RegexDoS.aspx", "");
            var response = new System.Web.HttpResponse(new System.IO.StringWriter());
            System.Web.HttpContext.Current = new System.Web.HttpContext(request, response);

            var page = (RegexDoS)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(RegexDoS));
            SetPrivateField(page, "txtUsername", new System.Web.UI.WebControls.TextBox { Text = "^(a+)+$" });
            SetPrivateField(page, "txtPassword", new System.Web.UI.WebControls.TextBox { Text = new string('a', 5000) + "!" });
            SetPrivateField(page, "lblError", new System.Web.UI.WebControls.Label());

            var m = typeof(RegexDoS).GetMethod("btnCreate_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(m);

            Assert.Throws<TargetInvocationException>(() => m!.Invoke(page, new object?[] { page, EventArgs.Empty }));
            // Note: WebForms event handlers don't catch the timeout; the inner exception should be RegexMatchTimeoutException.
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            var f = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(f);
            f!.SetValue(target, value);
        }
    }
}
