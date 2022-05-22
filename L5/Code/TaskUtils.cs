using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;

namespace L5.Code
{
    public class TaskUtils
    {
        public static void AssignDuration(List<Server> Emails, List<Server> ServerSpecifications, Table errorTable)
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

        public static int TransferDuration(double TransferSpeed, double fileSize)
        {
            return Convert.ToInt32(fileSize / TransferSpeed);
        }

        public static double GetServerSpeed(string Servername, List<Server> serversSpecs)
        {
            //try
            {
                return serversSpecs.FirstOrDefault(s => s.Name == Servername).InterfaceSpeed;
            }
            //catch(NullReferenceException )
            //{
            //    throw new Exception($"{Servername} serveris nera palaikomas.");
            //}
            
        }

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
        public static double BytesTransferedInHour(Server server, int hour)
        {
            return server.EmailTraffic.Where(s => s.SendTime.Hour == hour)
                                      .Select(m => m.SizeInBytes)
                                      .Sum();

        }
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