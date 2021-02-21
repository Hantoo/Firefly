using Newtonsoft.Json;
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
        [JsonIgnore]
        private int _activeImageSlot { get; set; }
        [JsonIgnore]
        public int activeImageSlot { get { return _activeImageSlot; } set {
                if (flagEmergencyAtNode) { _activeImageSlot = 9; } else if (isExit)
                {
                    _activeImageSlot = 10;
                } else { _activeImageSlot = value; }
            }}
        public int orientationRotation { get; set; }
        public int emergecnyImage { get; set; }
        //Used For Quick Node Networking 
        public List<int> deviceConnectionOutputIds = new List<int>();

        //Routing Variables
        public bool hasBeenRouted = false;
        public int routingType = 0; // 0 = Linear (As The Bird Flies), 1 = Step (Across And Down)
        //public bool isDeactivated { get; set; }
        private bool _isExit { get; set; }
        public bool isExit
        {
            get { return _isExit; }
            set { _isExit = value; if (_isExit == true) { defaultImage = 10; } else { defaultImage = 0; } }
        }
        public int defaultImage { get { return _defaultImage; } set { _defaultImage = value; if (!ServerResources.ServerManagement.shouldEvacuate) { activeImageSlot = _defaultImage; } } }
        [JsonIgnore]
        private int _defaultImage { get; set; }
        //public bool isRoom = false;
        [JsonIgnore]
        public bool hasHeartBeat { get; set; }
        [JsonIgnore]
        public bool isRunningRoutine { get; set; }
        [JsonIgnore]
        public int heartbeatRefreshCount { get; set; }

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


        public DeviceModel()
        {
          
            activeImageSlot = defaultImage;
        }

    }
}
