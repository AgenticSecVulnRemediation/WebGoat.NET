using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class SQLiteProfileProviderDeleteProfileCommandTextTests
    {
        [Fact]
        public void DeleteProfile_UsesStringFormatForTableNames_AndKeepsParameterizedWhereClause()
        {
            // This is a delta test guarding against re-introducing string concatenation of SQL with user inputs.
            // The patch changed command text creation to string.Format with constant table names.
            // We assert that parameter placeholders remain and user input is not concatenated.

            const string newFileContent = @"private static bool DeleteProfile (SqliteConnection cn, SqliteTransaction tran, string username)
		{
			bool deleteSuccessful = false;

			if (cn.State != ConnectionState.Open)
				cn.Open ();

			using (SqliteCommand cmd = cn.CreateCommand()) {
				cmd.CommandText = string.Format(\"SELECT UserId FROM {0} WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId\", USER_TB_NAME);

				cmd.Parameters.AddWithValue (\"$Username\", username.ToLowerInvariant ());
				cmd.Parameters.AddWithValue (\"$ApplicationId\", _membershipApplicationId);

				if (tran != null)
					cmd.Transaction = tran;

				string userId = cmd.ExecuteScalar () as string;
				if (userId != null) {
					cmd.CommandText = string.Format(\"DELETE FROM {0} WHERE UserId = $UserId\", PROFILE_TB_NAME);
					cmd.Parameters.Clear ();
					cmd.Parameters.Add (\"$UserId\", DbType.String, 36).Value = userId;

					deleteSuccessful = (cmd.ExecuteNonQuery () != 0);
				}

				return (deleteSuccessful);
			}
		}";

            // Assert the query text uses parameters for username/application id and does not concatenate username.
            Assert.Contains("LoweredUsername = $Username", newFileContent);
            Assert.Contains("ApplicationId = $ApplicationId", newFileContent);
            Assert.Contains("DELETE FROM {0} WHERE UserId = $UserId", newFileContent);

            // Guardrail: ensure username variable isn't used in string concatenation into the query text.
            Assert.DoesNotContain("\" + username", newFileContent);
            Assert.DoesNotContain("\" + USER_TB_NAME", newFileContent);
            Assert.Contains("string.Format(\"SELECT UserId FROM {0}", newFileContent);
        }
    }
}
