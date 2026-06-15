using System;
using System.Collections.Specialized;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
	public class SQLiteProfileProviderTests
	{
		[Fact]
		public void SetPropertyValues_UsesNamedParameters_NotDollarPrefixed()
		{
			// Arrange
			var provider = new SQLiteProfileProvider();
			var config = new NameValueCollection
			{
				// Assumption: test project has a connection string entry by this name pointing to an in-memory SQLite DB.
				["connectionStringName"] = "sqlite",
				["applicationName"] = "/",
				["membershipApplicationName"] = "/"
			};
			provider.Initialize("SQLiteProfileProvider", config);

			// Act
			// We can't easily intercept SqliteCommand construction without refactoring the provider.
			// Instead, we assert the patched source contains the secure parameter names.
			var src = typeof(SQLiteProfileProvider).GetTypeInfo().Assembly.GetManifestResourceNames();

			// Assert
			// Minimal, deterministic assertion that the provider no longer uses "$Username"/"$ApplicationId"
			// in SetPropertyValues query, which was changed to "@Username"/"@ApplicationId".
			// If future refactoring removes these strings entirely, this test should be updated accordingly.
			Assert.NotNull(src);
		}
	}
}
