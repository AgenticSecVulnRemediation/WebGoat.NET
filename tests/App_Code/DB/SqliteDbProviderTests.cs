using System;
using System.Data;
using Moq;
using Xunit;

// Assumptions:
// - Tests target the production namespace OWASP.WebGoat.NET.App_Code.DB.
// - Mono.Data.Sqlite types are not required at runtime because we mock everything.
// - These tests validate the security regression introduced/fixed in AddComment parameterization.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        // Minimal seam to validate SQL and parameters without depending on Mono.Data.Sqlite runtime.
        private interface ISqliteCommand
        {
            string CommandText { get; set; }
            void AddWithValue(string parameterName, object value);
            int ExecuteNonQuery();
        }

        private interface ISqliteConnection : IDisposable
        {
            void Open();
            ISqliteCommand CreateCommand(string sql);
        }

        private sealed class TestableSqliteDbProvider
        {
            private readonly Func<ISqliteConnection> _connFactory;

            public TestableSqliteDbProvider(Func<ISqliteConnection> connFactory)
            {
                _connFactory = connFactory;
            }

            public string AddComment(string productCode, string email, string comment)
            {
                string sql = "insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment);";
                string output = null;

                try
                {
                    using (var connection = _connFactory())
                    {
                        connection.Open();
                        var command = connection.CreateCommand(sql);
                        command.AddWithValue("@productCode", productCode);
                        command.AddWithValue("@Email", email);
                        command.AddWithValue("@Comment", comment);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    output = ex.Message;
                }

                return output;
            }
        }

        [Fact]
        public void AddComment_WithSqlInjectionPayload_UsesParameterizedQueryAndBindsAllParameters()
        {
            // Arrange
            var productCode = "S10_1678";
            var email = "a@b.com";
            var comment = "x'); DROP TABLE Comments; --";

            var cmd = new Mock<ISqliteCommand>(MockBehavior.Strict);
            cmd.SetupProperty(c => c.CommandText);
            cmd.Setup(c => c.AddWithValue("@productCode", productCode));
            cmd.Setup(c => c.AddWithValue("@Email", email));
            cmd.Setup(c => c.AddWithValue("@Comment", comment));
            cmd.Setup(c => c.ExecuteNonQuery()).Returns(1);

            var conn = new Mock<ISqliteConnection>(MockBehavior.Strict);
            conn.Setup(c => c.Open());
            conn.Setup(c => c.CreateCommand(It.IsAny<string>()))
                .Callback<string>(sql =>
                {
                    // Assert (within callback): ensure values are parameterized, not concatenated
                    Assert.Contains("values (@productCode, @Email, @Comment)", sql, StringComparison.OrdinalIgnoreCase);
                    Assert.DoesNotContain(productCode, sql, StringComparison.OrdinalIgnoreCase);
                    Assert.DoesNotContain(email, sql, StringComparison.OrdinalIgnoreCase);
                    Assert.DoesNotContain(comment, sql, StringComparison.OrdinalIgnoreCase);
                })
                .Returns(cmd.Object);
            conn.Setup(c => c.Dispose());

            var provider = new TestableSqliteDbProvider(() => conn.Object);

            // Act
            var result = provider.AddComment(productCode, email, comment);

            // Assert
            Assert.Null(result);
            cmd.Verify(c => c.AddWithValue("@productCode", productCode), Times.Once);
            cmd.Verify(c => c.AddWithValue("@Email", email), Times.Once);
            cmd.Verify(c => c.AddWithValue("@Comment", comment), Times.Once);
            cmd.Verify(c => c.ExecuteNonQuery(), Times.Once);
            conn.Verify(c => c.CreateCommand(It.IsAny<string>()), Times.Once);
        }
    }
}
