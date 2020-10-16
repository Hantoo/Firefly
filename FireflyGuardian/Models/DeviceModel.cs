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
        public (double, double, double) deviceXYZLocation;
        public (double, double) deviceXZLocationOnGrid;
        public string deviceIP { get; set; }
        public int deviceID { get; set; }
        public List<int> deviceIDConnectionsOutput = new List<int>();
        //Assigned To based on outputs from other devices targetting this device.
        public List<int> deviceIDConnectionsInput = new List<int>();
        //Used For Quick Node Networking 
        public List<DeviceModel> deviceConnectionsOutput = new List<DeviceModel>();
        //Assigned To based on outputs from other devices targetting this device.
        public List<DeviceModel> deviceConnectionsInput = new List<DeviceModel>();

    }
}
