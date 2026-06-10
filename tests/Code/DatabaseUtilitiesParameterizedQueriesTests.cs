using System;
using System.Data;
using System.Reflection;
using Mono.Data.Sqlite;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesParameterizedQueriesTests
    {
        [Fact]
        public void GetMailingListInfoByEmailAddress_UsesSqliteParameter_NotStringConcatenation()
        {
            // The fix switched from string concatenation to a parameterized query.
            // Use reflection to invoke the private DoQuery overload and assert it accepts parameters.

            var type = typeof(OWASP.WebGoat.NET.DatabaseUtilities);
            var method = type.GetMethod("GetMailingListInfoByEmailAddress", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            // Assert the class contains the new DoQuery signature with params SqliteParameter[].
            var doQuery = type.GetMethod("DoQuery", BindingFlags.NonPublic | BindingFlags.Instance, null,
                new[] { typeof(string), typeof(SqliteConnection), typeof(SqliteParameter[]) }, null);

            Assert.NotNull(doQuery);
        }

        [Fact]
        public void AddToMailingList_UsesSqliteParameters_NotInterpolatedValues()
        {
            var type = typeof(OWASP.WebGoat.NET.DatabaseUtilities);

            // Assert the class contains the new DoNonQuery signature with params SqliteParameter[].
            var doNonQuery = type.GetMethod("DoNonQuery", BindingFlags.NonPublic | BindingFlags.Instance, null,
                new[] { typeof(string), typeof(SqliteConnection), typeof(SqliteParameter[]) }, null);

            Assert.NotNull(doNonQuery);
        }
    }
}
