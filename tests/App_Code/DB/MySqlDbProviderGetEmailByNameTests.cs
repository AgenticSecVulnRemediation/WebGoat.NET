using System;
using System.Reflection;
using Moq;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesParameter_WithTrailingWildcard()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns("dummy");
            var method = typeof(MySqlDbProvider).GetMethod("GetEmailByName");

            // Assert
            Assert.NotNull(method);
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            var markerParam = System.Text.Encoding.UTF8.GetBytes("@name");
            Assert.Contains(markerParam, new ReadOnlySpan<byte>(il).ToArray());
            var markerWildcard = System.Text.Encoding.UTF8.GetBytes("%" );
            Assert.Contains(markerWildcard, new ReadOnlySpan<byte>(il).ToArray());
        }
    }
}
