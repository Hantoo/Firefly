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
using System.Net.NetworkInformation;

namespace FireflyGuardian.ServerResources.UDP
{
    class UDPServer
    {
        //ToDo: Set Polling of devices to boardcast on IP
        private UdpClient udpClient;
        private const int sendPort = 42891; //43594;
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

        public IPAddress[] GetBroadCastIP()
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            Console.WriteLine(hostName);
            // Get the IP  
            string[] ipInterfaceAddresses = new string[Dns.GetHostByName(hostName).AddressList.Length];
            IPAddress[] boardcastInterfaceAddresses = new IPAddress[Dns.GetHostByName(hostName).AddressList.Length];

            for (int i = 0; i < Dns.GetHostByName(hostName).AddressList.Length; i++)
            {
                ipInterfaceAddresses[i] = Dns.GetHostByName(hostName).AddressList[i].ToString();
                string myIP = Dns.GetHostByName(hostName).AddressList[i].ToString();
                

                Console.WriteLine(Dns.GetHostByName(hostName));
                IPAddress host = IPAddress.Parse(myIP);
                IPAddress mask = IPAddress.Parse("255.255.0.0");
                try
                {
                    mask = GetSubnetMask(host);
                }
                catch(Exception e){ }
                byte[] broadcastIPBytes = new byte[4];
                byte[] hostBytes = host.GetAddressBytes();
                byte[] maskBytes = mask.GetAddressBytes();
                for (int j = 0; j < 4; j++)
                {
                    broadcastIPBytes[j] = (byte)(hostBytes[j] | (byte)~maskBytes[j]);
                }
                boardcastInterfaceAddresses[i] = new IPAddress(broadcastIPBytes);
            }
            
            return boardcastInterfaceAddresses;
        }


        public static IPAddress GetSubnetMask(IPAddress address)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (address.Equals(unicastIPAddressInformation.Address))
                        {
                            return unicastIPAddressInformation.IPv4Mask;
                        }
                    }
                }
            }
            throw new ArgumentException(string.Format("Can't find subnetmask for IP address '{0}'", address));
        } //Subnet mask code: http://www.java2s.com/Code/CSharp/Network/GetSubnetMask.htm

        public void UDPSend(byte[] message, string iP, int portAddress = sendPort)
        {
            try
            {
                if (iP == "" || message.Length == 0) { return; }
                udpDataModel sendMessage = new udpDataModel();
                sendMessage.data = message;
                sendMessage.altIPData = new IPEndPoint(utils.GetLocalIPAddress(), sendPort);
                sendMessage.ipData = new IPEndPoint(IPAddress.Parse(iP), portAddress);
                sendDataQueue.Enqueue(sendMessage);
                //ipLogModel.addToLog(sendMessage, true);
                UDPEvent?.Invoke(this, "Message");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:" + ex);
                return;
            }

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
