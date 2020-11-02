using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using FireflyGuardian.Models;

namespace FireflyGuardian.ServerResources.UDP
{
    public class UDPMessageRouting
    {
        //public event EventHandler<DeviceModel> DevicePollResponseEvent;

        public void routeMessage(udpDataModel udpModel)
        {
            byte[] message = udpModel.data;
            /*if (message[message.Length-1] != 0x00)
            {
                //Message is formatted wrongly - Expected 0xFF as a start byte but didn't get that.
                return;
            }
            else
            {
                byte[] decodedMessage = Utils.utils.DecodeConsistentOverheadByteStuffing(message);*/
                //Message formatted correctly
                switch (message[1])
                {
                    case 0xA0:
                        readPollResponse(message);
                        break;

                    case 0xA9:
                        returnServerInfomation(udpModel.altIPData);
                        break;
                }
            //}
        }

        public void readPollResponse(byte[] message)
        {
            int lengthOfMessage = utils.ConvertLoHiBytesToInt(message[2], message[3]);
            int deviceID = utils.ConvertLoHiBytesToInt(message[4], message[5]);
            string IP = ((int)message[6]).ToString() + "." + ((int)message[7]).ToString() + "." + ((int)message[8]).ToString() + "." + ((int)message[9]).ToString();
            //int numberOfImagesOnDevice = message[10];
            int lengthOfName = message[10]; //Utils.utils.ConvertLoHiBytesToInt(message[10]);
            byte[] byteName = new byte[lengthOfName];
            for (int i = 0; i < lengthOfName; i++)
            {
                byteName[i] = message[i + 11];
            }
            string name = Encoding.UTF8.GetString(byteName, 0, byteName.Length);
            // DeviceInfomationModel device = createNewDevice(deviceID, IP, name);
            // DevicePollResponseEvent?.Invoke(this, device);
            DeviceModel device = new DeviceModel();
            device.deviceIP = IP;
            device.deviceName = name;
            device.deviceID = deviceID;
            ServerManagement.addDevice(device);
            

            //for(int i = 0;)
        }

        public void returnServerInfomation(IPEndPoint ipData)
        {
            
            IPAddress address = ipData.Address;
            int port = ipData.Port;
            IPAddress thisIP = utils.GetLocalIPAddress();
            
            byte[] thisIPBytes = thisIP.GetAddressBytes();
            byte[] message = new byte[thisIPBytes.Length + 2];
            message[0] = 0xFF;
            message[1] = 0x04;
            System.Array.Copy(thisIPBytes, 0, message, 2, thisIPBytes.Length);
            //System.Array.Copy(thisPort, 0, message, message.Length- thisPort.Length, thisPort.Length);
            //ShellViewModel.udpConnection.UDPSend(message, address.ToString(), port);
        }

       /* public DeviceInfomationModel createNewDevice(int deviceID, string iP, string name = "" )
        {
            DeviceInfomationModel newDevice = new DeviceInfomationModel();
            newDevice.deviceId = deviceID;
            newDevice.ip = iP;
            newDevice.name = name;
            return newDevice;
        }*/

    }


}
