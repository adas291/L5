using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L5.Code
{
    public class Email 
    {
        public string SenderAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public int SizeInBytes { get; set; }
        public DateTime SendTime { get; set; }

        public Email(string senderAddress, string receiverAddress, int sizeInBytes, DateTime sendTime)
        {
            SenderAddress = senderAddress;
            ReceiverAddress = receiverAddress;
            SizeInBytes = sizeInBytes;
            SendTime = sendTime;
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }
}