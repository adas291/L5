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

        public Server(string name, double interfaceSpeed)
        {
            Name = name;
            InterfaceSpeed = interfaceSpeed;
        }

        public void AddEmail(Email email)
        {
            EmailTraffic.Add(email);
        }
    }
}