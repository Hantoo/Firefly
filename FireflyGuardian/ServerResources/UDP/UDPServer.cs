using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using FireflyGuardian.Models;

namespace FireflyGuardian.ServerResources.UDP
{
    class UDPServer
    {

        private UdpClient udpClient;
        private const int sendPort = 43594;
        private const int listenPort = 43595;
        public int listenport { get { return listenPort; } }
        private Queue<udpDataModel> recvDataQueue = new Queue<udpDataModel>();
        public event EventHandler<string> recievedUDPEvent;
        public event EventHandler<string> UDPEvent;
        public Queue<udpDataModel> sendDataQueue = new Queue<udpDataModel>();
        public const int SIO_UDP_CONNRESET = -1744830452;

        //public IPLogModel ipLogModel;
        public UDPMessageRouting messageRouting;
        public Thread recvThread;
        private static volatile bool runRecvecingThread;

        public UDPServer()
        {
            //ipLogModel = new IPLogModel();
            recvDataQueue = new Queue<udpDataModel>();
            udpClient = new UdpClient(listenPort);
            udpClient.Client.IOControl((IOControlCode)SIO_UDP_CONNRESET, new byte[] { 0, 0, 0, 0 }, null);
            messageRouting = new UDPMessageRouting();
            runRecvecingThread = true;
            recvThread = new Thread(new ThreadStart(ThreadedRecv));
            recvThread.Start();

            this.recievedUDPEvent += UDPConnectionModel_recievedUDPEvent;
        }

        public static void shutdownUDP()
        {
            runRecvecingThread = false;
        }

        private void UDPConnectionModel_recievedUDPEvent(object sender, string e)
        {
            
            if (recvDataQueue.Count > 0)
            {
                int responseNums = recvDataQueue.Count;
                for (int i = 0; i < responseNums; i++)
                {
                    udpDataModel message = recvDataQueue.Dequeue();
                    //ipLogModel.addToLog(message, false);
                    messageRouting.routeMessage(message);
                }
            }
        }


        public void UDPSend(byte[] message, string iP, int portAddress = sendPort)
        {
            udpDataModel sendMessage = new udpDataModel();
            sendMessage.data = message;
            sendMessage.altIPData = new IPEndPoint(utils.GetLocalIPAddress(), sendPort);
            sendMessage.ipData = new IPEndPoint(IPAddress.Parse(iP), portAddress);
            sendDataQueue.Enqueue(sendMessage);
            //ipLogModel.addToLog(sendMessage, true);
            UDPEvent?.Invoke(this, "Message");

        }

        public void UDPBroadcast(byte[] message)
        {

            UDPSend(message, "255.255.255.255", sendPort);

        }

        /*public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }*/

        public void ThreadedRecv()
        {
            udpDataModel messageToSend;
            while (runRecvecingThread)
            {
                try
                {
                    if (sendDataQueue.Count > 0)
                    {
                        messageToSend = sendDataQueue.Dequeue();
                        if (messageToSend != null)
                        {
                            if (messageToSend.data != null && messageToSend.ipData != null)
                            {
                                udpClient.Send(messageToSend.data, messageToSend.data.Length, messageToSend.ipData);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("UDP Error: " + e.ToString());
                }
                try
                {
                    if (udpClient.Available > 0)
                    {
                        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        udpDataModel packetData = new udpDataModel();

                        packetData.data = udpClient.Receive(ref RemoteIpEndPoint);
                        packetData.altIPData = RemoteIpEndPoint;
                        packetData.ipData = new IPEndPoint(utils.GetLocalIPAddress(), sendPort);
                        recvDataQueue.Enqueue(packetData);
                        
                        //Invoke udp listen way if someone is listening to the event.
                        recievedUDPEvent?.Invoke(this, "Message");
                        UDPEvent?.Invoke(this, "Message");
                    }
                    // do any background work
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Recviving Thread Error: " + ex.ToString());
                    break;
                    // log errors
                }
            }
        }


    }


}
