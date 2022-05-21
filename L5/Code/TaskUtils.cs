using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;

namespace L5.Code
{
    public class TaskUtils
    {
        public static void AssignDuration(List<Server> Emails, List<Server> servers)
        {
            foreach (var server in Emails)
            {
                foreach (var email in server.EmailTraffic)
                {
                    double speed = GetServerSpeed(server.Name, servers);
                    if(speed != 0)
                    {
                        email.TransferDuration = TransferDuration(speed, email.SizeInBytes);
                    }
                    else
                    {
                        email.TransferDuration = -1;
                    }
                }
            }
        }

        public static int TransferDuration(double TransferSpeed, int fileSize)
        {
            return Convert.ToInt32(fileSize / TransferSpeed);
        }

        public static double GetServerSpeed(string Servername, List<Server> serversSpecs)
        {
            try
            {
                return serversSpecs.FirstOrDefault(s => s.Name == Servername).InterfaceSpeed;
            }
            catch(NullReferenceException )
            {
                throw new Exception("Nera tokio serverio sarase");
            }
            
        }

        public static List<Tuple<string, DateTime, int>> FindIdleHours(List<Server> servers)
        {
            List<Tuple<string, DateTime, int>> set = new List<Tuple<string, DateTime, int>>();

            foreach (var item in servers)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (BytesTransferedInHour(item, i) == 0)
                    {
                        Tuple<string, DateTime, int> temp = Tuple.Create(item.Name, item.TransferDate, i);
                        set.Add(temp);
                    }
                }
            }
            return set;
        }
        public static int BytesTransferedInHour(Server server, int hour)
        {
            //int totalQuota = 0;

            //Implement counting from previous hours if not transfered;
            return server.EmailTraffic.Where(s => s.SendTime.Hour == hour).Select(m => m.SizeInBytes).Sum();
            
        }
    }
}