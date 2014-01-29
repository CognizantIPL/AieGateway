using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace AIEGateway.Web.DataAccess
{
    public class Repository
    {
        private string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["AieDb"].ConnectionString;
            }
        }

        public IEnumerable<string> GetRegisteredDevices()
        {
            var channels = new List<string>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = conn.CreateCommand())
                {
                    conn.Open();

                    // Query the table and print the results
                    command.CommandText = "SELECT CHANNEL FROM AIEMOBILESERVICE.APP_DEVICE";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Loop over the results
                        while (reader.Read())
                        {
                            channels.Add(reader["CHANNEL"].ToString().Trim());
                        }
                    }

                    conn.Close();
                }
            }

            return channels;

        }
        
    }
}


/*

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Microsoft.SDS.Samples
{
class Program
{
// Provide the following information
private static string userName = "<ProvideUserName>";
private static string password = "<ProvidePassword>";
private static string dataSource = "<ProvideServerName>";
private static string sampleDatabaseName = "<ProvideDatabaseName>";

static void Main(string[] args)
{
    // Create a connection string for the master database
    SqlConnectionStringBuilder connString1Builder;
    connString1Builder = new SqlConnectionStringBuilder();
    connString1Builder.DataSource = dataSource;
    connString1Builder.InitialCatalog = "master";
    connString1Builder.Encrypt = true;
    connString1Builder.TrustServerCertificate = false;
    connString1Builder.UserID = userName;
    connString1Builder.Password = password;

    // Create a connection string for the sample database
    SqlConnectionStringBuilder connString2Builder;
    connString2Builder = new SqlConnectionStringBuilder();
    connString2Builder.DataSource = dataSource;
    connString2Builder.InitialCatalog = sampleDatabaseName;
    connString2Builder.Encrypt = true;
    connString2Builder.TrustServerCertificate = false;
    connString2Builder.UserID = userName;
    connString2Builder.Password = password;

    // Connect to the master database and create the sample database
    using (SqlConnection conn = new SqlConnection(connString1Builder.ToString()))
    {
        using (SqlCommand command = conn.CreateCommand())
        {

            conn.Open();

            // Create the sample database
            string cmdText = String.Format("CREATE DATABASE {0}",
                                            sampleDatabaseName);
            command.CommandText = cmdText;
            command.ExecuteNonQuery();
            conn.Close();
        }
    }

    // Connect to the sample database and perform various operations
    using (SqlConnection conn = new SqlConnection(connString2Builder.ToString()))
    {
        using (SqlCommand command = conn.CreateCommand())
        {
            conn.Open();

            // Create a table
            command.CommandText = "CREATE TABLE T1(Col1 int primary key, Col2 varchar(20))";
            command.ExecuteNonQuery();

            // Insert sample records
            command.CommandText = "INSERT INTO T1 (col1, col2) values (1, 'string 1'), (2, 'string 2'), (3, 'string 3')";
            int rowsAdded = command.ExecuteNonQuery();

            // Query the table and print the results
            command.CommandText = "SELECT * FROM T1";

            using (SqlDataReader reader = command.ExecuteReader())
            {
                // Loop over the results
                while (reader.Read())
                {
                    Console.WriteLine("Col1: {0}, Col2: {1}", 
                                    reader["Col1"].ToString().Trim(), 
                                    reader["Col2"].ToString().Trim());
                }
            }

            // Update a record
            command.CommandText = "UPDATE T1 SET Col2='string 1111' WHERE Col1=1";
            command.ExecuteNonQuery();

            // Delete a record
            command.CommandText = "DELETE FROM T1 WHERE Col1=2";
            command.ExecuteNonQuery();

            // Query the table and print the results

            Console.WriteLine("\nAfter update/delete the table has these records...");

            command.CommandText = "SELECT * FROM T1";

            using (SqlDataReader reader = command.ExecuteReader())
            {
                // Loop over the results
                while (reader.Read())
                {
                    Console.WriteLine("Col1: {0}, Col2: {1}", 
                                    reader["Col1"].ToString().Trim(), 
                                    reader["Col2"].ToString().Trim());
                }
            }

            conn.Close();
        }
    }
    Console.WriteLine("Press enter to continue...");
    Console.ReadLine();
}
}
}

*/