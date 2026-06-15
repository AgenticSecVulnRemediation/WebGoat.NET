using System;
using System.Collections.Specialized;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
	public class SQLiteMembershipProviderTests
	{
		[Fact]
		public void DeleteUser_DeleteAllRelatedData_DoesNotLeakParametersBetweenQueries()
		{
			// Arrange
			var provider = new SQLiteMembershipProvider();
			var config = new NameValueCollection
			{
				// Assumption: test project has a connection string entry by this name pointing to an in-memory SQLite DB.
				// This is consistent with other provider tests in this repo.
				["connectionStringName"] = "sqlite",
				["applicationName"] = "/",
				["requiresUniqueEmail"] = "false",
				["requiresQuestionAndAnswer"] = "false",
				["enablePasswordReset"] = "true",
				["enablePasswordRetrieval"] = "false",
				["minRequiredPasswordLength"] = "7",
				["minRequiredNonalphanumericCharacters"] = "1",
				["maxInvalidPasswordAttempts"] = "5",
				["passwordAttemptWindow"] = "10",
				["passwordStrengthRegularExpression"] = ""
			};

			provider.Initialize("SQLiteMembershipProvider", config);

			// Act / Assert
			// Regression test for the fix: DeleteUser previously reused the same SqliteCommand instance
			// without clearing parameters between the SELECT (UserId) and DELETE statements.
			// That could cause parameter collisions / unexpected behavior.
			var ex = Record.Exception(() => provider.DeleteUser("user@example.com", deleteAllRelatedData: true));
			Assert.Null(ex);
		}
	}
}
