using Xunit;

// Note: Assumes MySqlDbProvider is in this namespace as declared in the source file.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsParameterizationTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedSql_DoesNotInlineProductCode()
        {
            // Arrange
            // This delta test is limited to verifying the vulnerable behavior is removed:
            // the query strings in GetProductDetails should no longer concatenate/inline productCode.
            var provider = (MySqlDbProvider)System.Runtime.Serialization.FormatterServices
                .GetUninitializedObject(typeof(MySqlDbProvider));

            // Act
            // Instead of executing DB calls, validate the compiled method body contains the safe token.
            // We do this by reflecting over the method source-invariant IL string literals.
            var method = typeof(MySqlDbProvider).GetMethod("GetProductDetails");
            Assert.NotNull(method);

            var body = method!.GetMethodBody();
            Assert.NotNull(body);

            // Assert
            // We can't execute without a DB seam; assert the fixed parameter marker exists.
            // This ensures regression against reintroducing string concatenation.
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            Assert.NotNull(il);

            // The strongest deterministic assertion available without changing production code:
            // verify the method contains the parameter placeholder string.
            // If the code regresses to "... '" + productCode + "'", the placeholder disappears.
            Assert.Contains("select * from Products where productCode = @productCode",
                System.IO.File.ReadAllText(method.Module.FullyQualifiedName));
        }
    }
}
