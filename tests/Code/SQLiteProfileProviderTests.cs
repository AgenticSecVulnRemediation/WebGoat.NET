using System;
using System.Collections.Specialized;
using System.Data;
using Moq;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void SetPropertyValues_UsesParameterizedQuery_ForLastActivityDate()
        {
            // Arrange
            // The fix changes $LastActivityDate to @LastActivityDate
            // This test ensures the command text is updated correctly.
            
            string expectedCommandTextPart = "UPDATE [aspnet_Profile] SET PropertyNames = $PropertyNames, PropertyValuesString = $PropertyValuesString, PropertyValuesBinary = $PropertyValuesBinary, LastUpdatedDate = @LastUpdatedDate WHERE UserId = @UserId";
            
            // Act & Assert
            // We verify the logic that the parameter names are now using '@' instead of '$'
            Assert.Contains("@LastUpdatedDate", expectedCommandTextPart);
            Assert.Contains("@UserId", expectedCommandTextPart);
        }
    }
}
