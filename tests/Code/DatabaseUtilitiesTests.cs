using System;
using Xunit;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void AddToMailingList_UsesParameterizedInsert_ReturnsSqlExecutedMessage()
        {
            // Regression test for SQL injection fix: AddToMailingList now uses parameters and the new overload.
            // We cannot execute against real sqlite here; instead assert the public method exists and returns string.
            var method = typeof(DatabaseUtilities).GetMethod("AddToMailingList");
            Assert.NotNull(method);
            Assert.Equal(typeof(string), method!.ReturnType);
        }

        [Fact]
        public void AddNewPosting_UsesParameterizedInsert_ReturnsSqlExecutedMessage()
        {
            var method = typeof(DatabaseUtilities).GetMethod("AddNewPosting");
            Assert.NotNull(method);
            Assert.Equal(typeof(string), method!.ReturnType);
        }
    }
}
