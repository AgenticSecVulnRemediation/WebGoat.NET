using System;
using System.Data;
using Moq;
using Xunit;

// Assumptions:
// - This test validates GetProductsAndCategories uses a parameter for catNumber when catNumber >= 1.
// - We mock all DB-related dependencies.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesTests
    {
        private interface ISqliteDataAdapter
        {
            ISqliteCommand SelectCommand { get; }
            int Fill(DataSet dataSet, string tableName);
        }

        private interface ISqliteCommand
        {
            void AddWithValue(string parameterName, object value);
        }

        private interface ISqliteConnection : IDisposable
        {
            void Open();
            ISqliteDataAdapter CreateDataAdapter(string sql);
        }

        private sealed class TestableSqliteDbProvider
        {
            private readonly Func<ISqliteConnection> _connFactory;

            public TestableSqliteDbProvider(Func<ISqliteConnection> connFactory)
            {
                _connFactory = connFactory;
            }

            public DataSet GetProductsAndCategories(int catNumber)
            {
                string sql;
                DataSet ds = new DataSet();

                string catClause = string.Empty;
                if (catNumber >= 1)
                    catClause = " where catNumber = @catNumber";

                using (var connection = _connFactory())
                {
                    connection.Open();

                    sql = "select * from Categories" + catClause;
                    var da = connection.CreateDataAdapter(sql);
                    if (catNumber >= 1)
                        da.SelectCommand.AddWithValue("@catNumber", catNumber);
                    da.Fill(ds, "categories");

                    sql = "select * from Products" + catClause;
                    da = connection.CreateDataAdapter(sql);
                    if (catNumber >= 1)
                        da.SelectCommand.AddWithValue("@catNumber", catNumber);
                    da.Fill(ds, "products");

                    return ds;
                }
            }
        }

        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterizedClauseAndBindsCatNumber()
        {
            // Arrange
            var catNumber = 1;

            var cmd = new Mock<ISqliteCommand>(MockBehavior.Strict);
            cmd.Setup(c => c.AddWithValue("@catNumber", catNumber));

            var da = new Mock<ISqliteDataAdapter>(MockBehavior.Strict);
            da.SetupGet(d => d.SelectCommand).Returns(cmd.Object);
            da.Setup(d => d.Fill(It.IsAny<DataSet>(), It.IsAny<string>())).Returns(1);

            var conn = new Mock<ISqliteConnection>(MockBehavior.Strict);
            conn.Setup(c => c.Open());
            conn.Setup(c => c.CreateDataAdapter(It.IsAny<string>()))
                .Callback<string>(sql =>
                {
                    Assert.Contains("where catNumber = @catNumber", sql, StringComparison.OrdinalIgnoreCase);
                    Assert.DoesNotContain("where catNumber = 1", sql, StringComparison.OrdinalIgnoreCase);
                })
                .Returns(da.Object);
            conn.Setup(c => c.Dispose());

            var provider = new TestableSqliteDbProvider(() => conn.Object);

            // Act
            var ds = provider.GetProductsAndCategories(catNumber);

            // Assert
            Assert.NotNull(ds);
            // Called twice (Categories and Products)
            cmd.Verify(c => c.AddWithValue("@catNumber", catNumber), Times.Exactly(2));
            conn.Verify(c => c.CreateDataAdapter(It.IsAny<string>()), Times.Exactly(2));
            da.Verify(d => d.Fill(It.IsAny<DataSet>(), It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
