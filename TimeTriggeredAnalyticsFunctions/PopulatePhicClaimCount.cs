using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Eclaims.Analytics.Internal.Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace TimeTriggeredAnalyticsFunctions
{
    public static class PopulatePhicClaimCount
    {
        [FunctionName("PopulatePhicClaimCount")]
        public static void Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation("Triggered  PopulatePhicClaimCount ");
            var str = Environment.GetEnvironmentVariable("sqldb_connection");
            var phicclaimCountList = new List<PHICClaimCount>();
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                try
                {
                    var text = "Select CONVERT(DATE,a.dateupdated) as dateupdated ,b.hospitalcode ,c.region,c.name, " +
                        "COUNT(CASE WHEN a.claimstatus = 4 THEN 1 END) as InProcess, " +
                        "COUNT(CASE WHEN a.claimstatus = 5 THEN 1 END) as [Return], " +
                        "COUNT(CASE WHEN a.claimstatus = 6 THEN 1 END) as Denied, " +
                        "COUNT(CASE WHEN a.claimstatus = 7 THEN 1 END) as WithCheque, " +
                        "COUNT(CASE WHEN a.claimstatus = 8 THEN 1 END) as WithVoucher, " +
                        "COUNT(CASE WHEN a.claimstatus = 9 THEN 1 END) as Vouchering, " +
                        "COUNT(*) as TotalSubmitted " +
                        "from phicclaims as a " +
                        "inner join phictrasmittals as b on a.phictrasmittalid = b.id " +
                        "inner join clients as c on b.clientid = c.id Where a.claimstatus in (4, 5, 6, 7, 8, 9) " +
                        "Group By a.claimstatus,CONVERT(DATE, a.dateupdated),b.hospitalcode,c.region,c.name";

                    using (SqlCommand cmd = new SqlCommand(text, conn))
                    {
                        //    // Execute the command and log the # rows affected.
                        //    var rows = await cmd.ExecuteNonQueryAsync();
                        //    log.LogInformation($"{rows} rows were updated");
                        SqlDataReader reader = cmd.ExecuteReader();
                        do
                        {
                            int count = reader.FieldCount;

                            while (reader.Read())
                            {

                                var entry = new PHICClaimCount
                                {
                                    DateSubmitted = Convert.ToDateTime(reader.GetValue(0)),
                                    PMCC = reader.GetString(1),
                                    Region = reader.GetString(2),
                                    ClientName = reader.GetString(3),
                                    InProcess = Convert.ToInt32(reader.GetValue(4)),
                                    Returned = Convert.ToInt32(reader.GetValue(5)),
                                    Denied = Convert.ToInt32(reader.GetValue(6)),
                                    WithCheque = Convert.ToInt32(reader.GetValue(7)),
                                    WithVoucher = Convert.ToInt32(reader.GetValue(8)),
                                    Vouchering = Convert.ToInt32(reader.GetValue(9)),
                                    TotalClaimCount = Convert.ToInt32(reader.GetValue(10))
                                };

                                phicclaimCountList.Add(entry);
                            }

                        } while (reader.NextResult());

                    }

                }
                catch (Exception error)
                {

                }
            }
        }
    }
}
