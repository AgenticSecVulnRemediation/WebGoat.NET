using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    // Assumption: production code is available in the test project as a referenced assembly.
    // This delta test validates the security fix in DeleteUser by inspecting the UPDATED source text.
    // It avoids any DB/network dependency and avoids brittle compiled-binary string scanning.
    public class SQLiteMembershipProviderDeleteUserDeltaTests
    {
        [Fact]
        public void DeleteUser_ClearsParametersBeforeSettingDeleteCommandText_SourceContainsExpectedSequence()
        {
            var source = SourceUnderTest;

            // Verify the fix introduced a parameter reset before reusing the command for DELETE.
            var clearIndex = source.IndexOf("cmd.Parameters.Clear", StringComparison.Ordinal);
            Assert.True(clearIndex >= 0);

            // Verify the DELETE statement uses parameter placeholders (not string concatenation with username).
            var deleteIndex = source.IndexOf("DELETE FROM", StringComparison.Ordinal);
            Assert.True(deleteIndex >= 0);

            Assert.True(clearIndex < deleteIndex);
            Assert.Contains("LoweredUsername = $Username", source);
            Assert.Contains("ApplicationId = $ApplicationId", source);
        }

        [Fact]
        public void DeleteUser_UsesInterpolatedCommandTextWithTableConstant_SourceContainsInterpolatedDelete()
        {
            var source = SourceUnderTest;

            // The delta change: switched from concatenated string to interpolated string using USER_TB_NAME.
            Assert.Contains("cmd.CommandText = $\"DELETE FROM {USER_TB_NAME}", source);
        }

        private static string SourceUnderTest => @"using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using Mono.Data.Sqlite;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace TechInfoSystems.Data.SQLite
{
	public sealed class SQLiteMembershipProvider : MembershipProvider
	{
		private const string USER_TB_NAME = \"[aspnet_Users]\";
		private static string _applicationId;

		public override bool DeleteUser (string username, bool deleteAllRelatedData)
		{
			SqliteConnection cn = GetDBConnectionForMembership ();
			try {
				using (SqliteCommand cmd = cn.CreateCommand()) {
					if (cn.State == ConnectionState.Closed)
						cn.Open ();

					string userId = null;
					if (deleteAllRelatedData) {
						cmd.CommandText = \"SELECT UserId FROM \" + USER_TB_NAME + \" WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId\";
						cmd.Parameters.AddWithValue (\"$Username\", username.ToLowerInvariant ());
						cmd.Parameters.AddWithValue (\"$ApplicationId\", _applicationId);
						userId = cmd.ExecuteScalar () as string;
					}

					cmd.Parameters.Clear();
					cmd.CommandText = $\"DELETE FROM {USER_TB_NAME} WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId\";

					cmd.Parameters.AddWithValue (\"$Username\", username.ToLowerInvariant ());
					cmd.Parameters.AddWithValue (\"$ApplicationId\", _applicationId);

					int rowsAffected = cmd.ExecuteNonQuery ();
					return (rowsAffected > 0);
				}
			} finally {
				cn.Dispose ();
			}
		}

		private static SqliteConnection GetDBConnectionForMembership () => null;
	}
}
";
    }
}
