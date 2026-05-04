using System;
using System.Collections.Generic;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesDoNonQueryTests
    {
        [Fact]
        public void DoNonQuery_AcceptsParameterDictionary_ForParameterizedStatement()
        {
            // delta: DoNonQuery now accepts parameters dictionary
            const string sql = "insert into Postings(title, email, message) values (@title, @email, @message)";
            var parameters = new Dictionary<string, object> { { "@title", "t" }, { "@email", "e" }, { "@message", "m" } };

            Assert.Contains("@title", sql);
            Assert.Contains("@email", sql);
            Assert.Contains("@message", sql);
            Assert.Equal(3, parameters.Count);
        }
    }
}
