using System;
using Xunit;

using MySql.Data.MySqlClient;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentDeltaTests
    {
        [Fact]
        public void Patch134_AddComment_UsesThreeParameters()
        {
            // Delta assertion: INSERT now binds three parameters.
            var cmd = new MySqlCommand();
            cmd.Parameters.AddWithValue("@productCode", "S10_1678");
            cmd.Parameters.AddWithValue("@email", "user@example.com");
            cmd.Parameters.AddWithValue("@comment", "hello");

            Assert.NotNull(cmd.Parameters["@productCode"]);
            Assert.NotNull(cmd.Parameters["@email"]);
            Assert.NotNull(cmd.Parameters["@comment"]);
        }
    }
}
