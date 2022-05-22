using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L5.Code
{
    /// <summary>
    /// Class for saving information about servers
    /// </summary>
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
        /// <summary>
        /// For text file format
        /// </summary>
        /// <returns>Formated string</returns>
        public override string ToString()
        {
            return $"|{Name, -29}|{InterfaceSpeed, 15}|";
        }
    }
}