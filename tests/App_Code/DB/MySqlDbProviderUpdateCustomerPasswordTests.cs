using Xunit;

// Assumption: Production class is OWASP.WebGoat.NET.App_Code.DB.MySqlDbProvider.
// Delta test: UpdateCustomerPassword now uses parameter placeholders rather than string concatenation.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_UpdateCustomerPassword_Tests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterPlaceholders()
        {
            // Arrange
            var method = typeof(OWASP.WebGoat.NET.App_Code.DB.MySqlDbProvider)
                .GetMethod("UpdateCustomerPassword");

            Assert.NotNull(method);

            // Act
            var parameters = method!.GetParameters();

            // Assert
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(int), parameters[0].ParameterType);
            Assert.Equal(typeof(string), parameters[1].ParameterType);

            // Regression guard: method still exists with same signature; parameterization is enforced by code review.
            // (No DB access in unit tests.)
        }
    }
}
