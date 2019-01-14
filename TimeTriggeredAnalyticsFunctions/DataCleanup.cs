using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TimeTriggeredAnalyticsFunctions
{
    public static class DataCleanup
    {
        [FunctionName("DataCleanup")]
        public static async Task Run([TimerTrigger("*/10 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            // Get the connection string from app settings and use it to create a connection.
            var str = Environment.GetEnvironmentVariable("sqldb_connection");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                try
                {
                    //var text = "SELECT * FROM Branches;";
                    //using (SqlCommand cmd = new SqlCommand(text, conn))
                    //{
                    ////    // Execute the command and log the # rows affected.
                    ////    var rows = await cmd.ExecuteNonQueryAsync();
                    ////    log.LogInformation($"{rows} rows were updated");
                    //    SqlDataReader reader = cmd.ExecuteReader();
                    //    while (reader.Read())
                    //    {
                    //        var weight = reader.GetString(0);    // Weight int
                    //        var name = reader.GetString(1);  // Name string
                    //        var breed = reader.GetString(2); // Breed string 
                    //        var breeda = reader.GetDateTime(3); // Breed string 
                    //        log.LogInformation(string.Format("{0}, {1}, {2} {3}", weight, name, breed, breeda));
                    //    }
                    //}
                     
                    using (SqlCommand cmd = new SqlCommand("Insert into  Branches VALUES(@Id, @Name, @Address, @Date, @Hospital_Id)", conn))
                    {
                        //    // Execute the command and log the # rows affected.
                        //    var rows = await cmd.ExecuteNonQueryAsync();
                        //    log.LogInformation($"{rows} rows were updated");
                        cmd.Parameters.Add(new SqlParameter("Id", "Test"));
                        cmd.Parameters.Add(new SqlParameter("Name", "Azure"));
                        cmd.Parameters.Add(new SqlParameter("Address", "Function"));
                        cmd.Parameters.Add(new SqlParameter("Date", new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)));
                        cmd.Parameters.Add(new SqlParameter("Hospital_Id", "1000"));
                        cmd.ExecuteNonQuery();
                        log.LogInformation("Insert Done"); 
                    }

                }
                catch (Exception error)
                { 
                    log.LogInformation(error.Message.ToString());
                }
            }
        }
    }
}
