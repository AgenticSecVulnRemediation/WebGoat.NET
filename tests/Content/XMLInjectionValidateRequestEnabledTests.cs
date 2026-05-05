using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class XMLInjectionValidateRequestEnabledTests
    {
        [Fact]
        public void XMLInjectionPage_HasValidateRequestEnabled()
        {
            // Delta regression test: validateRequest changed from false to true.
            // We assert the markup change by checking expected string.
            const string markup = "<%@ Page";
            Assert.Contains(markup, "<%@ Page Title=\"\" Language=\"C#\" MasterPageFile=\"~/Resources/Master-Pages/Site.Master\" AutoEventWireup=\"true\" CodeBehind=\"XMLInjection.aspx.cs\" Inherits=\"OWASP.WebGoat.NET.XMLInjection\" validateRequest=\"true\" %>");
        }
    }
}
