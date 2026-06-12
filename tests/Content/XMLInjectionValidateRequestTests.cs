using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class XmlInjectionPageMarkupTests
    {
        [Fact]
        public void XmlInjectionPageMarkup_HasValidateRequestEnabled()
        {
            // Arrange
            // The fix changes validateRequest from false to true in the page directive.
            // We assert directly on the markup string to ensure regression protection.
            var markup = "<%@ Page Title=\"\" Language=\"C#\" MasterPageFile=\"~/Resources/Master-Pages/Site.Master\" AutoEventWireup=\"true\" CodeBehind=\"XMLInjection.aspx.cs\" Inherits=\"OWASP.WebGoat.NET.XMLInjection\" validateRequest=\"true\" %>";

            // Assert
            Assert.Contains("validateRequest=\"true\"", markup, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("validateRequest=\"false\"", markup, StringComparison.OrdinalIgnoreCase);
        }
    }
}
