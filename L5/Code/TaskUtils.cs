using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;

namespace L5.Code
{
    /// <summary>
    /// Class for calculations of server data
    /// </summary>
    public class TaskUtils
    {
        /// <summary>
        /// Assigns durations that it took to send email based on server
        /// speed
        /// </summary>
        /// <param name="Emails"></param>
        /// <param name="ServerSpecifications"></param>
        /// <param name="errorTable"></param>
        public static void AssignDuration(List<Server> Emails, List<Server> 
                                        ServerSpecifications, Table errorTable)
        {
            List<string> errors = new List<string>();

            foreach (var server in Emails)
            {
                try
                {
                    foreach (var email in server.EmailTraffic)
                    {
                        double speed = GetServerSpeed(server.Name, ServerSpecifications);
                        if (speed != 0)
                        {
                            email.TransferDuration = TransferDuration(speed, email.SizeInBytes);
                        }
                        else
                        {
                            email.TransferDuration = -1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                    continue;
                }
            }
            if (errors.Count > 0)
            {
                InOut.FillTable(errors, errorTable);
            }
        }
        /// <summary>
        /// Durations of email transmission in seconds
        /// </summary>
        /// <param name="TransferSpeed">Server interface speed</param>
        /// <param name="fileSize">email's size</param>
        /// <returns>duration in seconds</returns>
        public static int TransferDuration(double TransferSpeed, double fileSize)
        {
            return Convert.ToInt32(fileSize / TransferSpeed);
        }
        /// <summary>
        /// Returns server speed based on it's name
        /// </summary>
        /// <param name="Servername">server to search for</param>
        /// <param name="serversSpecs">all server's information</param>
        /// <returns>Server's speed in bytes per second</returns>
        public static double GetServerSpeed(string Servername, List<Server> serversSpecs)
        {
            {
                return serversSpecs.FirstOrDefault(s => s.Name == Servername).InterfaceSpeed;
            }            
        }
        /// <summary>
        /// Finds hours when servers are idle
        /// </summary>
        /// <param name="servers">All server's data</param>
        /// <returns>tuple with string date and hour number</returns>
        public static List<Tuple<string, DateTime, int>> FindIdleHours(List<Server> servers)
        {
            List<Tuple<string, DateTime, int>> set = new List<Tuple<string, DateTime, int>>();

            foreach (var item in servers)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (BytesTransferedInHour(item, i) == 0 && !NextHour(item.EmailTraffic, i-1))
                    {
                        Tuple<string, DateTime, int> temp = Tuple.Create(item.Name, item.TransferDate, i);
                        set.Add(temp);
                    }
                }
            }
            return set;
        }
        /// <summary>
        /// Returns server's transmission at one hour
        /// </summary>
        /// <param name="server">One server object</param>
        /// <param name="hour">specific hour to count bytes</param>
        /// <returns>sum of all bytes that server transfered</returns>
        public static double BytesTransferedInHour(Server server, int hour)
        {
            return server.EmailTraffic.Where(s => s.SendTime.Hour == hour)
                                      .Select(m => m.SizeInBytes)
                                      .Sum();
        }
        /// <summary>
        /// Checks if email transfered between two hours
        /// </summary>
        /// <param name="email">all emails of server</param>
        /// <param name="hour">specific transmission hour</param>
        /// <returns>True if email happened between two hours and
        /// vice versa</returns>
        public static bool NextHour(List<Email> email, int hour)
        {
            List<Email> correctHour = email.Where(s => s.SendTime.Hour == hour).ToList();

            foreach (var item in correctHour)
            {
                int tillHour = 60 - item.SendTime.Minute;
                if (tillHour < item.TransferDuration / 60)
                {
                    return true;
                }
            }
            return false;
        }
    }
}