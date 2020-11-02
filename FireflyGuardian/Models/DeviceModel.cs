using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireflyGuardian.Models
{

    class DeviceModel
    {
        public string deviceName { get; set; }
        //public (double, double, double) deviceXYZLocation;
        public (double, double) deviceXZLocationOnGrid;
        public string deviceIP { get; set; }
        public string deviceWifiIP { get; set; } //ToDo: Implement Wifi switching if can't connect to device
        public int deviceID { get; set; }
        public List<int> deviceIDConnectionsOutput = new List<int>();
        public bool flagEmergencyAtNode { get; set; }
        public int activeImageSlot { get; set; }
        //Assigned To based on outputs from other devices targetting this device.
        //public List<int> deviceIDConnectionsInput = new List<int>();
        //Used For Quick Node Networking 
        public List<int> deviceConnectionOutputIds = new List<int>();
        //Assigned To based on outputs from other devices targetting this device.
        //public List<DeviceModel> deviceConnectionsInput = new List<DeviceModel>();

        //Routing Variables
        public bool hasBeenRouted = false;
        public int routingType = 0; // 0 = Linear (As The Bird Flies), 1 = Step (Across And Down)
        public bool _isExit = false;
        public bool isExit { get { return _isExit; } set { _isExit = value; } }
        //public bool isRoom = false;


        //PathCalculationVariables
        public struct exitRouting
        {
            public int TargetExitNode;
            public int nextNodeID;
            public int directionToNextNode;
            public List<int> fullPath;
            public int distanceToTargetExitNode;

        }

        public List<exitRouting> exitRoutings = new List<exitRouting>();


    }
}
