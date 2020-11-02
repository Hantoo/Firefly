using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Threading;

namespace FireflyGuardian.Models
{
    
    public class udpDataModel
    {
        public byte[] data;
        public IPEndPoint ipData;
        public IPEndPoint altIPData;
    }
}
