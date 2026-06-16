using System;
using System.Reflection;
using Xunit;

using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesAddNewPostingTests
    {
        [Fact]
        public void AddNewPosting_MethodSignature_Unchanged()
        {
            // Arrange
            var method = typeof(DatabaseUtilities).GetMethod("AddNewPosting", new[] { typeof(string), typeof(string), typeof(string) });

            // Assert
            Assert.NotNull(method);
            Assert.Equal(typeof(string), method.ReturnType);
        }
    }
}
