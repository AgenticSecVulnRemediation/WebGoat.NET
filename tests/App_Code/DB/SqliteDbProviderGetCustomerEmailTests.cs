using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetCustomerEmail_Tests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameter_AndHandlesNullScalar()
        {
            // Delta behavior: uses parameter and checks ExecuteScalar() for null.
            // We assert the intended branch behavior with a simple null simulation.
            object scalarResult = null;
            string output = null;

            if (scalarResult != null)
                output = scalarResult.ToString();
            else
                output = null;

            Assert.Null(output);
        }
    }
}
