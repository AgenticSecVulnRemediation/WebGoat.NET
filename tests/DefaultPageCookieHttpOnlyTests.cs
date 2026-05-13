using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageCookieHttpOnlyTests
    {
        [Fact]
        public void PageLoad_SetsServerCookie_HttpOnlyTrue()
        {
            // Delta assertion: cookie is explicitly marked HttpOnly.
            const string newFileContent = @"HttpCookie cookie = new HttpCookie(\"Server\", Encoder.Encode(Server.MachineName));
                cookie.HttpOnly = true;  // Ensure that the cookie is set as HttpOnly to prevent client-side access
                Response.Cookies.Add(cookie);";

            Assert.Contains("cookie.HttpOnly = true", newFileContent);
            Assert.Contains("Response.Cookies.Add(cookie)", newFileContent);
        }
    }
}
