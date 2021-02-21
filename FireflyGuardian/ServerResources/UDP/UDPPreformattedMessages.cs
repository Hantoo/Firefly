using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using FireflyGuardian.Models;

namespace FireflyGuardian.ServerResources.UDP
{
    public static class UDPPreformattedMessages
    {

        public static void PollBoards()
        {
           
            Byte[] message = { 0xff, 0x01 };
            IPAddress[] addresses = ServerManagement.udpServer.GetBroadCastIP();
            for (int i = 0; i < addresses.Length; i++) {
                string ipAddressString = addresses[i].ToString();
                ServerManagement.udpServer.UDPSend(message, ipAddressString);
            }
        }

        public static void RequestImagesFromFTPForAllDevices()
        {

            Byte[] message = { 0xff, 0x05 };
            IPAddress[] addresses = ServerManagement.udpServer.GetBroadCastIP();
            for (int i = 0; i < addresses.Length; i++)
            {
                string ipAddressString = addresses[i].ToString();
                ServerManagement.udpServer.UDPSend(message, ipAddressString);
            }
        }

        public static void SetName(string Name, string ipAddressString)
        {
           
            byte[] nameBytes = Encoding.ASCII.GetBytes(Name);
            Console.WriteLine("Bytes:" + nameBytes.Length);
            byte[] message = new byte[4+ Name.Length];
            message[0] = 0xff;
            message[1] = 0x03;
            message[2] = 0x11;
            message[3] = (byte)(Name.Length);
            Array.Copy(nameBytes, 0, message,4, Name.Length);
            ServerManagement.udpServer.UDPSend(message, ipAddressString);
        }

        public static void SetEmergencyImage()
        {
            for(int i = 0; i < ServerManagement.devices.Count; i++)
            {
                byte[] message = new byte[4];
                message[0] = 0xff;
                message[1] = 0x03;
                message[2] = 0x18;
                message[3] = (byte)(ServerManagement.devices[i].emergecnyImage);
                ServerManagement.udpServer.UDPSend(message, ServerManagement.devices[i].deviceIP);
                Console.WriteLine(ServerManagement.devices[i].deviceIP + ", " + ServerManagement.devices[i].emergecnyImage);
            }
           
        }

        public static void SetOrientation(int Orientation, string ipAddressString)
        {
            Byte[] message = { 0xff, 0x03, 0x17, (byte)Orientation};
            ServerManagement.udpServer.UDPSend(message, ipAddressString);
            
        }

    }
}
