using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class User
    {
        public Socket socket { get; set; }
        public EndPoint endPoint { get; set; }
    }
}
