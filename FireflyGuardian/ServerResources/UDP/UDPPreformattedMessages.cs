using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireflyGuardian.ServerResources.UDP
{
    public static class UDPPreformattedMessages
    {

        public static void PollBoards()
        {
           
            Byte[] message = { 0xff, 0x01 };
            ServerManagement.udpServer.UDPSend(message, "255.255.255.255");
            //MessageBox.Show("Sent");
        }

    }
}
