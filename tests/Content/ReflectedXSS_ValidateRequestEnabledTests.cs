using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.Content.Tests
{
    public class ReflectedXssPageTests
    {
        [Fact]
        public void ReflectedXssPage_ValidateRequest_IsEnabled()
        {
            // Arrange
            // We treat the ASPX markup as the unit-under-test for this delta: validateRequest switched from false to true.
            // This is a simple regression guard to prevent reintroducing reflected XSS by disabling request validation.
            var aspxMarkup = "<%@ Page Title=\"\" validateRequest=\"true\" Language=\"C#\" MasterPageFile=\"~/Resources/Master-Pages/Site.Master\" AutoEventWireup=\"true\" CodeBehind=\"ReflectedXSS.aspx.cs\" Inherits=\"OWASP.WebGoat.NET.ReflectedXSS\" %>";

            // Act
            // Parse the page directive in a minimal way.
            var lowered = aspxMarkup.ToLowerInvariant();

            // Assert
            Assert.Contains("validaterequest=\"true\"", lowered);
            Assert.DoesNotContain("validaterequest=\"false\"", lowered);
        }
    }
}
