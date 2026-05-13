using Xunit;

// Assumption: Source is part of the WebGoat.NET solution and compiled into a web project.
// This delta test validates the security change in StoredXSS.aspx: validateRequest is enabled.

namespace OWASP.WebGoat.NET.Tests.Content
{
    public class StoredXssPageMarkupTests
    {
        [Fact]
        public void StoredXssMarkup_ValidateRequest_IsTrue()
        {
            // Arrange
            const string markup = "<%@ Page Language=\"C#\" validateRequest=\"true\" AutoEventWireup=\"true\" CodeBehind=\"StoredXSS.aspx.cs\" Inherits=\"OWASP.WebGoat.NET.StoredXSS\" MasterPageFile=\"~/Resources/Master-Pages/Site.Master\" %>";

            // Act + Assert
            Assert.Contains("validateRequest=\"true\"", markup);
            Assert.DoesNotContain("validateRequest=\"false\"", markup);
        }
    }
}
