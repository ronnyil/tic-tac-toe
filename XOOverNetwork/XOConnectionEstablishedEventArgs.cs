using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace XOOverNetwork
{
    public class XOConnectionEstablishedEventArgs : EventArgs
    {
        public IPAddress RemoteIPAddress { get; set; }
        public int RemotePort { get; set; }
    }
}
