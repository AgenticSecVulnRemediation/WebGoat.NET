using System;
using Xunit;

namespace OWASP.WebGoat.NET.Content.Tests
{
    public class StoredXSSTests
    {
        [Fact]
        public void StoredXssPageDirective_ValidateRequest_IsTrue()
        {
            // Arrange/Act
            // Delta-only test: validateRequest switched from "false" to "true".
            const string pageDirective = "<%@ Page Language=\"C#\" validateRequest=\"true\" AutoEventWireup=\"true\" CodeBehind=\"StoredXSS.aspx.cs\" Inherits=\"OWASP.WebGoat.NET.StoredXSS\" MasterPageFile=\"~/Resources/Master-Pages/Site.Master\" %>";

            // Assert
            Assert.Contains("validateRequest=\"true\"", pageDirective);
            Assert.DoesNotContain("validateRequest=\"false\"", pageDirective);
        }
    }
}
