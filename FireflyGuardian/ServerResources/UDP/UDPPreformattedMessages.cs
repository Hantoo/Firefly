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
            //ToDo: Make it so that on click, it wipes any current polls. E.g. so you don't end up with duplicates.
            //UDPServer.messageRouting.DevicePollResponseEvent += addToQueue;
            Byte[] message = { 0xff, 0x01 };
            ServerManagement.udpServer.UDPSend(message, "255.255.255.255");
            //MessageBox.Show("Sent");
        }

    }
}
