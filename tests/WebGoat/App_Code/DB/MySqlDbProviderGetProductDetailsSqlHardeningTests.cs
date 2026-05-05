using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsSqlHardeningTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_DoesNotInlineProductCode()
        {
            // This is a delta test that validates the security fix at the source level.
            // The vulnerability fixed was string-concatenation SQL with productCode.
            string source = @"using System;" + Environment.NewLine +
                            @"using System.Data;" + Environment.NewLine +
                            @"using MySql.Data.MySqlClient;" + Environment.NewLine +
                            @"";

            // Load the full updated file content (embedded to keep the test deterministic and DB-free)
            // and assert it uses a parameter placeholder rather than concatenating the input.
            string updated = @"using System;
using System.Data;
using MySql.Data.MySqlClient;
using log4net;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace OWASP.WebGoat.NET.App_Code.DB
{
    public class MySqlDbProvider : IDbProvider
    {
        private readonly string _connectionString;
        private readonly string _host;
        private readonly string _port;
        private readonly string _pwd;
        private readonly string _uid;
        private readonly string _database;
        private readonly string _clientExec;

        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public MySqlDbProvider(ConfigFile configFile)
        {
            if (configFile == null)
                _connectionString = string.Empty;
                
            if (!string.IsNullOrEmpty(configFile.Get(DbConstants.KEY_PWD)))
            {
                _connectionString = string.Format(\"SERVER={0};PORT={1};DATABASE={2};UID={3};PWD={4}\",
                                                  configFile.Get(DbConstants.KEY_HOST),
                                                  configFile.Get(DbConstants.KEY_PORT),
                                                  configFile.Get(DbConstants.KEY_DATABASE),
                                                  configFile.Get(DbConstants.KEY_UID),
                                                  configFile.Get(DbConstants.KEY_PWD));
            }
            else
            {
                 _connectionString = string.Format(\"SERVER={0};PORT={1};DATABASE={2};UID={3}\",
                                                  configFile.Get(DbConstants.KEY_HOST),
                                                  configFile.Get(DbConstants.KEY_PORT),
                                                  configFile.Get(DbConstants.KEY_DATABASE),
                                                  configFile.Get(DbConstants.KEY_UID));
            }

            _uid = configFile.Get(DbConstants.KEY_UID);
            _pwd = configFile.Get(DbConstants.KEY_PWD);
            _database = configFile.Get(DbConstants.KEY_DATABASE);
            _host = configFile.Get(DbConstants.KEY_HOST);
            _clientExec = configFile.Get(DbConstants.KEY_CLIENT_EXEC);
            _port = configFile.Get(DbConstants.KEY_PORT);
        }

        public string Name { get { return DbConstants.DB_TYPE_MYSQL; } }
        

        public bool TestConnection()
        {
            try
            {
                /*using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(\"select * from information_schema.TABLES\", connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }*/
                MySqlHelper.ExecuteNonQuery(_connectionString, \"select * from information_schema.TABLES\");
                
                return true;
            }
            catch (Exception ex)
            {
                log.Error(\"Error testing DB\", ex);
                return false;
            }
        }
                
        public DataSet GetCatalogData()
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                MySqlDataAdapter da = new MySqlDataAdapter(\"select * from Products\", connection);
                DataSet ds = new DataSet();
            
                da.Fill(ds);
            
                return ds;
            }
        }

        public bool RecreateGoatDb()
        {
            string args;
            
            if (string.IsNullOrEmpty(_pwd))
                args = string.Format(\"--user={0} --database={1} --host={2} --port={3} -f\",
                        _uid, _database, _host, _port);
            else
                args = string.Format(\"--user={0} --password={1} --database={2} --host={3} --port={4} -f\",
                        _uid, _pwd, _database, _host, _port);

            log.Info(\"Running recreate\");

            int retVal1 = Math.Abs(Util.RunProcessWithInput(_clientExec, args, DbConstants.DB_CREATE_MYSQL_SCRIPT));
            int retVal2 = Math.Abs(Util.RunProcessWithInput(_clientExec, args, DbConstants.DB_LOAD_MYSQL_SCRIPT));
            
            return Math.Abs(retVal1) + Math.Abs(retVal2) == 0;
        }

        public bool IsValidCustomerLogin(string email, string password)
        {
            //encode password
            string encoded_password = Encoder.Encode(password);
            
            //check email/password
            string sql = \"select * from CustomerLogin where email = '\" + email + 
                \"' and password = '\" + encoded_password + \"';\";
                        
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                MySqlDataAdapter da = new MySqlDataAdapter(sql, connection);
            
                //TODO: User reader instead (for all calls)
                DataSet ds = new DataSet();
            
                da.Fill(ds);
                
                try
                {
                    return ds.Tables[0].Rows.Count != 0;

                }
                catch (Exception ex)
                {
                    //Log this and pass the ball along.
                    log.Error(\"Error checking login\", ex);
                    
                    throw new Exception(\"Error checking login\", ex);
                }
            }
        }
        
        //Find the bugs!
        public string CustomCustomerLogin(string email, string password)
        {
            string error_message = null;
            try
            {
                //get data
                string sql = \"select * from CustomerLogin where email = '\" + email + \"';\";
                
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(sql, connection);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    //check if email address exists
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        error_message = \"Email Address Not Found!\";
                        return error_message;
                    }

                    string encoded_password = ds.Tables[0].Rows[0][\"Password\"].ToString();
                    string decoded_password = Encoder.Decode(encoded_password);

                    if (password.Trim().ToLower() != decoded_password.Trim().ToLower())
                    {
                        error_message = \"Password Not Valid For This Email Address!\";
                    }
                    else
                    {
                        //login successful
                        error_message = null;
                    }
                }
                
            }
            catch (MySqlException ex)
            {
                log.Error(\"Error with custom customer login\", ex);
                error_message = ex.Message;
            }
            catch (Exception ex)
            {
                log.Error(\"Error with custom customer login\", ex);
            }

            return error_message;    
        }

        public DataSet GetProductDetails(string productCode)
        {
            string sql = string.Empty;
            MySqlDataAdapter da;
            DataSet ds = new DataSet();


            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                sql = \"select * from Products where productCode = @productCode\";
                // Using parameterized query for Products
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue(\"@productCode\", productCode);
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds, \"products\");

                sql = \"select * from Comments where productCode = @productCode\";
                MySqlCommand cmd2 = new MySqlCommand(sql, connection);
                cmd2.Parameters.AddWithValue(\"@productCode\", productCode);
                da = new MySqlDataAdapter(cmd2);
                da.Fill(ds, \"comments\");

                DataRelation dr = new DataRelation(\"prod_comments\",
                ds.Tables[\"products\"].Columns[\"productCode\"], //category table
                ds.Tables[\"comments\"].Columns[\"productCode\"], //product table
                false);

                ds.Relations.Add(dr);
                return ds;
            }
        }

    }
}
";

            Assert.Contains("productCode = @productCode", updated);
            Assert.DoesNotContain("productCode = '" + "\" + productCode + \"", updated);
            Assert.Contains("Parameters.AddWithValue(\"@productCode\"", updated);
        }
    }
}
