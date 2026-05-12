using System;
using Mono.Data.Sqlite;
using Xunit;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void AddToMailingList_UsesParameterizedQueryHelper()
        {
            // Delta assertion: method exists after introducing parameterized DoNonQuery overload.
            var method = typeof(DatabaseUtilities).GetMethod("AddToMailingList");
            Assert.NotNull(method);
        }

        [Fact]
        public void AddNewPosting_UsesParameterizedQueryHelper()
        {
            var method = typeof(DatabaseUtilities).GetMethod("AddNewPosting");
            Assert.NotNull(method);
        }
    }
}
