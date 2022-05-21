using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L5.Code
{
    public class Server
    {
        public string Name { get; set; }
        public double InterfaceSpeed { get; set; }
        public List<Email> EmailTraffic { get; set; }
        public DateTime TransferDate { get; set; }

        public Server(string name, double interfaceSpeed)
        {
            Name = name;
            InterfaceSpeed = interfaceSpeed;
        }

        public Server(string name, List<Email> emailTraffic, DateTime transferDate)
        {
            Name = name;
            EmailTraffic = emailTraffic;
            TransferDate = transferDate;
        }

        public void AddEmail(Email email)
        {
            EmailTraffic.Add(email);
        }
        public override string ToString()
        {
            return $"|{Name, -29}|{InterfaceSpeed, 15}|";
        }
    }
}