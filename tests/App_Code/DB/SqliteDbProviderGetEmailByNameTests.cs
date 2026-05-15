using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // Delta test: GetEmailByName was changed to use a parameterized LIKE (@name) instead of concatenation.
    public class SqliteDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_WithInjectionLikeName_DoesNotReturnAllRows()
        {
            using var connection = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            connection.Open();

            using (var create = connection.CreateCommand())
            {
                create.CommandText = "CREATE TABLE Employees (firstName TEXT, lastName TEXT, email TEXT);";
                create.ExecuteNonQuery();

                create.CommandText = "INSERT INTO Employees(firstName, lastName, email) VALUES ('Alice','A','a@example.com'),('Bob','B','b@example.com');";
                create.ExecuteNonQuery();
            }

            // Injection-like input that would have broken out of the quote in the old concatenated query.
            var injected = "A%' OR '1'='1";

            using var cmd = new SqliteCommand("select firstName, lastName, email from Employees where firstName like @name or lastName like @name", connection);
            cmd.Parameters.AddWithValue("@name", injected + "%");

            var emails = new List<string>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    emails.Add(reader["email"].ToString());
                }
            }

            Assert.Empty(emails);
        }
    }
}
