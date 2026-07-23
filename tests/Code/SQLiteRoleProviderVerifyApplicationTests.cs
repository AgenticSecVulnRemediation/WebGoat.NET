using System;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderVerifyApplicationTests
    {
        [Fact]
        public void Initialize_WithEmptyConfig_ThrowsArgumentNullException()
        {
            // Delta context: VerifyApplication insert statement was adjusted, but Initialize still guards config null.
            // This test ensures provider still rejects null config (regression safety around changed area).

            var provider = new SQLiteRoleProvider();
            Assert.Throws<ArgumentNullException>(() => provider.Initialize("SQLiteRoleProvider", null));
        }
    }
}
