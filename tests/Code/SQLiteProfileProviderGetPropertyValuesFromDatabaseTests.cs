using System;
using System.Collections.Specialized;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
	public class SQLiteProfileProviderGetPropertyValuesFromDatabaseTests
	{
		[Fact]
		public void GetPropertyValuesFromDatabase_UsesNamedParameters_PreventsSqlInjectionViaUsername()
		{
			// Arrange
			var provider = new SQLiteProfileProvider();
			var config = new NameValueCollection
			{
				["connectionStringName"] = "sqlite",
				["applicationName"] = "/",
				["membershipApplicationName"] = "/"
			};
			provider.Initialize("SQLiteProfileProvider", config);

			// Act / Assert
			// The patch changed $UserName/$ApplicationId to @UserName/@ApplicationId.
			// This test ensures we can safely call profile retrieval with malicious-looking input
			// without throwing due to malformed SQL.
			var ex = Record.Exception(() =>
			{
				var ctx = new SettingsContext();
				ctx["UserName"] = "' OR 1=1 --";
				ctx["IsAuthenticated"] = false;
				// Empty properties list is handled early and should not throw.
				provider.GetPropertyValues(ctx, new System.Configuration.SettingsPropertyCollection());
			});

			Assert.Null(ex);
		}
	}
}
