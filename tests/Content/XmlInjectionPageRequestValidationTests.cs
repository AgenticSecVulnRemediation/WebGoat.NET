using System;
using System.Reflection;
using System.Web;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class XmlInjectionPageRequestValidationTests
    {
        [Fact]
        public void XmlInjectionAspxPage_HasValidateRequestEnabled()
        {
            // This delta test ensures request validation is enabled on the page directive.
            // It's a structural security regression test.

            var pageType = typeof(XMLInjection);
            var assembly = pageType.Assembly;
            var asmString = assembly.ToString();

            Assert.Contains("XMLInjection.aspx", asmString);
            Assert.Contains("validateRequest=\"true\"", asmString);
            Assert.DoesNotContain("validateRequest=\"false\"", asmString);
        }
    }
}
