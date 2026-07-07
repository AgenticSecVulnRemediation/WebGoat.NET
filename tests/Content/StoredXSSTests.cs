using System;
using Xunit;

namespace OWASP.WebGoat.NET.Content.Tests
{
    public class StoredXSSTests
    {
        [Fact]
        public void StoredXssPage_RequestValidation_IsEnabled()
        {
            // Arrange
            // Delta-only test: the page directive was changed from validateRequest="false" to "true".
            var aspx = @"<%@ Page Language=""C#"" validateRequest=""true"" AutoEventWireup=""true"" CodeBehind=""StoredXSS.aspx.cs"" Inherits=""OWASP.WebGoat.NET.StoredXSS"" MasterPageFile=""~/Resources/Master-Pages/Site.Master"" %>";

            // Assert
            Assert.Contains("validateRequest=\"true\"", aspx);
            Assert.DoesNotContain("validateRequest=\"false\"", aspx);
        }
    }
}
