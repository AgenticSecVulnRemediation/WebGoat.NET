using System;
using System.Text;
using Xunit;

using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesParameterizationTests
    {
        [Fact]
        public void AddNewPosting_UsesSqliteParameters()
        {
            // Delta regression test: AddNewPosting now uses @title/@email/@message parameters and DoNonQuery accepts params SqliteParameter[].
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(DatabaseUtilities).Assembly.Location));

            Assert.Contains("values (@title, @email, @message)", asmText);
            Assert.Contains("new SqliteParameter(\"@title\"", asmText);
            Assert.Contains("params SqliteParameter[]", asmText);
        }
    }
}
