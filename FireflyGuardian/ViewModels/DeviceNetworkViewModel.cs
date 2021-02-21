using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireflyGuardian.Models;
using System.Windows;
using System.Windows.Controls;
using FireflyGuardian.ServerResources.DataAccess;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;
using FireflyGuardian.ServerResources;
using System.ComponentModel;
using FireflyGuardian.Views;

namespace FireflyGuardian.ViewModels
{

    //ToDo: Multiple Seclection Of Devices  - https://stackoverflow.com/questions/5741161/how-to-bind-multiple-selection-of-listview-to-viewmodel  https://stackoverflow.com/questions/2282138/wpf-listview-selecting-multiple-list-view-items
    //ToDo: Allow user to set device infomation from server and it to be updated on device.
    //ToDo: Assigning Distance And Capacity Of Corridor 

    //ToDo: Assigning Fire Or No Go Area
    //ToDo: Assigning Node As Room
    //ToDo: Add Removal Of Connections
    public delegate void NotifyCanvasRefresh();
    public delegate void NotifyNodeChange();
    public delegate void NotifyDeviceViewModelDestory();

    public enum infoWindowShown
    {
        deviceInfomation,
        addDevice,
        editDevice,
        none
    }

    class DeviceNetworkViewModel : Screen
    {
        private BindableCollection<DeviceModel> _deviceList = new BindableCollection<DeviceModel>();
        public BindableCollection<DeviceModel> deviceList { get { return _deviceList; } set { _deviceList = value; } }
        public static event NotifyCanvasRefresh RefreshCanvas;
        public static event NotifyDeviceViewModelDestory DestroyCanvas;
        private NodeRouting routes;
        public static event Action<int> UserChangedNode;
        private int _statusBannerHeight = 0;
        public int statusBannerHeight { get { return _statusBannerHeight; } set { _statusBannerHeight = value; } }
        public string statusText { get; set; }
        public bool statusShown { get; set; }
        DeviceNodeGraphViewModel nodeGraph;
        public bool canvasDraga { get; set; }
        public bool nodeDrag { get; set; }
        public int globalUpdateTimeRef { get; set; }
        public int heartUpdateTimeRef
        { get; set; }

        public int defaultImageSlot { get { if (SelectedDevice != null) { return SelectedDevice.defaultImage; Console.WriteLine("Selected Device Exists"); } else { return 0; } }
            set { if (SelectedDevice != null) { Console.WriteLine("Selected Device Exists"); SelectedDevice.defaultImage = value; NotifyOfPropertyChange(() => SelectedDevice); } } }
        private int _selectedDeviceIndex;
        private DeviceModel _SelectedDevice;
        public BindableCollection<DeviceModel> _SelectedDeviceConnections = new BindableCollection<DeviceModel>();
        public BindableCollection<DeviceModel> SelectedDeviceConnections { get { return _SelectedDeviceConnections; } }

        public double deviceLocationX { get; set; }
        public double deviceLocationZ { get; set; }
        public int routingType { get; set; }
        public int Orientation { get; set; }
        private infoWindowShown _infoWindow { get; set; }
        public infoWindowShown infoWindow { get { return _infoWindow; } set { previnfoWindow = _infoWindow; _infoWindow = value; NotifyOfPropertyChange(() => infoWindow); } }
        public infoWindowShown previnfoWindow { get; set; }
        public string evacButtonColour { get; set;}
        //public bool nodeDrag { get { return nodeDrag; } set { nodeDrag = value; if (value == true) { canvasDraga = false; } } }
        //public bool canvasDraga { get { return canvasDraga; } set { canvasDraga = value; if (value == true) { nodeDrag = false; } } }
        //public object NodeGraphViewModelView { get; set; }// = new DeviceNodeGraphViewModel();
        public DeviceNetworkViewModel()
        {
            infoWindow = infoWindowShown.none;
            nodeGraph = new DeviceNodeGraphViewModel();
            /*MessageBox.Show(ServerResources.ServerManagement.devices.Count.ToString());*/
            //NodeGraphViewModel = new DeviceNodeGraphViewModel();

            NotifyOfPropertyChange(() => canvasDraga);
            FireflyGuardian.Views.DeviceNodeGraphView.UserToggledCanvasDrag += toggleUserCanvasDrag;
            FireflyGuardian.Views.DeviceNodeGraphView.UserToggledNodeDrag += toggleUserNodeDrag;
            FireflyGuardian.Views.DeviceNodeGraphView.UserSelectedNode += setSelectedDeviceFromGraph;
            FireflyGuardian.Views.DeviceNodeGraphView.AutoUpdateList += autoUpdateList;
            FireflyGuardian.ViewModels.ShellViewModel.NotfiyNewView += ViewScreen;
            FireflyGuardian.ViewModels.ShellViewModel.NotfiyDestoryView += DestoryScreen;

            if (ServerManagement.shouldEvacuate)
            {
                evacButtonColour = "#A01313"; NotifyOfPropertyChange(() => evacButtonColour);
            }
            else { evacButtonColour = "#3E3E3E"; NotifyOfPropertyChange(() => evacButtonColour); }
        }

        public void ViewScreen()
        {
            if (FireflyGuardian.ViewModels.ShellViewModel.activePageType == this.GetType())
            {
                updateDeviceListWindow();
            }
        }
        public void DestoryScreen()
        {
            if (FireflyGuardian.ViewModels.ShellViewModel.activePageType == this.GetType())
            {
                DestroyCanvas.Invoke();
            }

        }

        List<DeviceModel> compareDevices = new List<DeviceModel>();
        public void RefreshDevices()
        {
            compareDevices = new List<DeviceModel>(ServerManagement.devices);
            ServerManagement.clearAllDevice();
            deviceList.Clear();
            ServerResources.UDP.UDPPreformattedMessages.PollBoards();
            statusShown = true;
            statusText = "Please Wait For Devices To Be Polled (10s)";
            statusBannerHeight = 30;
            NotifyOfPropertyChange(() => statusText);
            NotifyOfPropertyChange(() => statusShown);
            NotifyOfPropertyChange(() => statusBannerHeight);
            Console.WriteLine("Count: " + compareDevices.Count);
            AwaitTaskDelay();
        }

        async Task PutTaskDelay()
        {
            await Task.Delay(10000);
        }

        async Task StatusBarCountDownFrom10(int num)
        {
            statusText = "Please Wait For Devices To Be Polled ("+(10-num)+"s)";
            NotifyOfPropertyChange(() => statusText);
            await Task.Delay(1000);
        }

        private async void AwaitTaskDelay()
        {
            for(int i = 0; i < 10; i++)
            {
                await StatusBarCountDownFrom10(i);
            }
            //await PutTaskDelay();
            statusShown = false;
            statusText = "";
            statusBannerHeight = 30;
            NotifyOfPropertyChange(() => statusText);
            NotifyOfPropertyChange(() => statusShown);
            NotifyOfPropertyChange(() => statusBannerHeight);
            updateDeviceListWindow();
            //deviceList = new BindableCollection<DeviceModel>(ServerManagement.devices);
            //NotifyOfPropertyChange(() => deviceList);
            for (int j = 0; j < compareDevices.Count; j++)
            {
                bool existsFromPoll = false;
                for (int i=0; i<ServerManagement.devices.Count; i++)
                {
                    if(ServerManagement.devices[i].deviceID == compareDevices[j].deviceID)
                    {
                        existsFromPoll = true;
                        ServerManagement.devices[i].deviceName = compareDevices[j].deviceName;
                        ServerManagement.devices[i].deviceXZLocationOnGrid = compareDevices[j].deviceXZLocationOnGrid;
                        ServerManagement.devices[i].activeImageSlot = compareDevices[j].activeImageSlot;
                        ServerManagement.devices[i].defaultImage = compareDevices[j].defaultImage;
                        ServerManagement.devices[i].deviceConnectionOutputIds = compareDevices[j].deviceConnectionOutputIds;
                        ServerManagement.devices[i].isExit = compareDevices[j].isExit;
                        ServerManagement.devices[i].orientationRotation = compareDevices[j].orientationRotation;
                        break;
                        //Polled Device is the same as previously added device
                    }
                }
                if (!existsFromPoll)
                {
                    ServerManagement.addDevice(compareDevices[j]);
                }
            }
            RefreshCanvas?.Invoke();
            compareDevices.Clear();
            Console.WriteLine("Devices Updated");

        }



        public static void GlobalDestroyNodeCanvas()
        {
            try
            {
                if(DestroyCanvas != null)
                DestroyCanvas.Invoke();
            }
            catch { }
        }

        public void DeactivateNode()
        {
           
            
            SelectedDevice.flagEmergencyAtNode = !SelectedDevice.flagEmergencyAtNode;
            if (SelectedDevice.flagEmergencyAtNode)
            {
                SelectedDevice.activeImageSlot = 9;
            }
            else
            {
                SelectedDevice.activeImageSlot = 0;
            }
            RefreshCanvas.Invoke();
        }

        public void resetAllNodes()
        {
            for(int i = 0; i< ServerManagement.devices.Count; i++)
            {
                ServerManagement.devices[i].flagEmergencyAtNode = false;
            }
        }


        public void toggleUserCanvasDrag()
        {
            canvasDraga = !canvasDraga;
            nodeDrag = false;
            NotifyOfPropertyChange(() => canvasDraga);
            NotifyOfPropertyChange(() => nodeDrag);
        }
        public void toggleUserNodeDrag()
        {
            nodeDrag = !nodeDrag;
            canvasDraga = false;
            NotifyOfPropertyChange(() => nodeDrag);
            NotifyOfPropertyChange(() => canvasDraga);
        }

        private void updateDeviceListWindow()
        {
            _deviceList.Clear();
            deviceList = new BindableCollection<DeviceModel>(ServerResources.ServerManagement.devices);
            //for (int i = 0; i < ServerResources.ServerManagement.devices.Count; i++)
            //{
            //    _deviceList.Add(ServerResources.ServerManagement.devices[i]);
            //}
            NotifyOfPropertyChange(() => _deviceList);
            NotifyOfPropertyChange(() => deviceList);
            RefreshCanvas?.Invoke();
        }
        
        public DeviceModel SelectedDevice
        {
            get { return _SelectedDevice; }
            //On New selected Device Do
            set
            {
                _SelectedDevice = (value);
                _SelectedDeviceConnections.Clear();
                //Calculate Connections. If has device connection IDs, scroll through device list and add them.
                for (int i = 0; i < SelectedDevice.deviceConnectionOutputIds.Count; i++)
                {
                    for(int j = 0; j < deviceList.Count; j++)
                    {
                        if(SelectedDevice.deviceConnectionOutputIds[i] == deviceList[j].deviceID)
                        {
                            _SelectedDeviceConnections.Add(deviceList[j]);
                        }
                    }
                    
                }
                if(value != null)
                {
                    infoWindow = infoWindowShown.deviceInfomation;
                    NotifyOfPropertyChange(() => infoWindow);
                    Console.WriteLine("Set Window Shown To Device Infomation");
                }
                else {
                    infoWindow = infoWindowShown.none;
                    NotifyOfPropertyChange(() => infoWindow);
                }
                defaultImageSlot = SelectedDevice.defaultImage;
                NotifyOfPropertyChange(() => defaultImageSlot);
                
                //Take double and split to single ints
                deviceLocationX = SelectedDevice.deviceXZLocationOnGrid.Item1;
                deviceLocationZ = SelectedDevice.deviceXZLocationOnGrid.Item2;

                routingType = SelectedDevice.routingType;

                NotifyOfPropertyChange(() => deviceLocationX);
                NotifyOfPropertyChange(() => deviceLocationZ);
                NotifyOfPropertyChange(() => routingType);
                NotifyOfPropertyChange(() => SelectedDeviceConnections);
                NotifyOfPropertyChange(() => SelectedDevice);
            }
        }

        private bool selectionTriggeredFromGraph;
        private void setSelectedDeviceFromGraph(int _id)
        {
            selectionTriggeredFromGraph = true;
            setSelectedDevice(_id);
        }


            public void setSelectedDevice(int DeviceID)
        {
            
            int idx = 0;
            for(int i = 0; i < ServerResources.ServerManagement.devices.Count; i++)
            {
                if(ServerResources.ServerManagement.devices[i].deviceID == DeviceID)
                {
                    idx = i;
                    break;
                }
            }
            
            selectedDeviceIndex = idx;
        }

        public int selectedDeviceIndex
        {
            get { return _selectedDeviceIndex; }
            set
            {

                if (deviceList.Count > 0)
                {
                    _selectedDeviceIndex = (value);
                    if (value >= 0)
                    {
                        SelectedDevice = (deviceList[value]);
                        NotifyOfPropertyChange(() => SelectedDevice);
                        NotifyOfPropertyChange(() => _selectedDeviceIndex);
                        NotifyOfPropertyChange(() => selectedDeviceIndex);
                        if (!selectionTriggeredFromGraph)
                        {
                            UserChangedNode.Invoke(SelectedDevice.deviceID);
                        }
                        selectionTriggeredFromGraph = false;
                    }
                }
            }
        }

        public void flagAsChangeToDeviceStructure(string message)
        {
            ServerManagement.deviceStructureValid = false;
            statusShown = true;
            statusText = message;
            statusBannerHeight = 30;
            NotifyOfPropertyChange(() => statusText);
            NotifyOfPropertyChange(() => statusShown);
            NotifyOfPropertyChange(() => statusBannerHeight);
            
        }

        




        public int selectedComboBoxItem { get; set; }

        public void saveSelectedDevice()
        {
            flagAsChangeToDeviceStructure("Recalculation Needed For New Node Location");
            SelectedDevice.deviceXZLocationOnGrid.Item1 = deviceLocationX;
            SelectedDevice.deviceXZLocationOnGrid.Item2 = deviceLocationZ;
            SelectedDevice.routingType = routingType;
            int selectedIndex = selectedDeviceIndex;
            deviceList[selectedIndex] = SelectedDevice;
            SelectedDevice = deviceList[selectedIndex];
            selectedDeviceIndex = selectedIndex;
            RefreshCanvas.Invoke();
        }

        public void AddToAdjacencyList()
        {

            if(selectedComboBoxItem != -1 && deviceList[selectedComboBoxItem].deviceID != SelectedDevice.deviceID)
            {
                SelectedDevice.deviceConnectionOutputIds.Add(deviceList[selectedComboBoxItem].deviceID);
            }
            flagAsChangeToDeviceStructure("Path Recalculation Needed For New Node");
           // saveSelectedDevice();

        }

        public void markNodeAsExit()
        {

            SelectedDevice.isExit = !SelectedDevice.isExit;
            
            flagAsChangeToDeviceStructure("Path Recalculation Needed For New Exit Node");
            NotifyOfPropertyChange(() => SelectedDevice);
            updateDeviceListWindow();
        }
        


        //SAVE AND LOAD 
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        Nullable<bool> hasFile;
        //Get JSON file and load it into the ServerManagement Variable for Devices.
        
        //Save the ServerManagement variable to 
        public void saveDevicesToJson()
        {
            
            json.saveDevices();
        }


        public void updateTimersText()
        {
            globalUpdateTimeRef = ServerManagement.nextGlobalUpdate_reference;
            heartUpdateTimeRef = ServerManagement.nextHeartBeatCheck_reference;
            NotifyOfPropertyChange(() => globalUpdateTimeRef);
            NotifyOfPropertyChange(() => heartUpdateTimeRef);
            
        }

        public void autoUpdateList()
        {

            deviceList = new BindableCollection<DeviceModel>(ServerManagement.devices);
            NotifyOfPropertyChange(() => deviceList);

            //If heartbeats have been checked then turn them off.
            for (int i = 0; i < ServerManagement.devices.Count; i++)
            {
                if (ServerManagement.devices[i].hasHeartBeat)
                {
                    if (ServerManagement.devices[i].heartbeatRefreshCount > 50)
                    {
                        ServerManagement.devices[i].hasHeartBeat = false;
                        ServerManagement.devices[i].heartbeatRefreshCount = 0;
                    }
                    else { ServerManagement.devices[i].heartbeatRefreshCount++; }

                }
            }
            updateTimersText();
        }
        //Routing 

        //Calculate Distance Between Nodes
        //Linear And Step

        public void recalculateRoutes()
        {
            routes = new NodeRouting();
            routes.makeGraphFromDeviceList(ServerManagement.devices);

            //Reset Header
            statusShown = false;
            statusText = "";
            statusBannerHeight = 0;
            NotifyOfPropertyChange(() => statusText);
            NotifyOfPropertyChange(() => statusShown);
            NotifyOfPropertyChange(() => statusBannerHeight);

            ServerManagement.deviceStructureValid = false;
            ServerResources.UDP.UDPPreformattedMessages.SetEmergencyImage();
        }

        public void RemoveConnection(string deletionDeviceID)
        {
            //obj
            //Button btn = sender as Button;
            int deletionDeviceIDint = int.Parse(deletionDeviceID);
            for(int i =0; i < SelectedDeviceConnections.Count; i++)
            {
                if(SelectedDeviceConnections[i].deviceID == deletionDeviceIDint)
                {
                    SelectedDeviceConnections.RemoveAt(i);
                }
            }
            
            for (int i =0; i < SelectedDevice.deviceConnectionOutputIds.Count; i++)
            {
                if(SelectedDevice.deviceConnectionOutputIds[i] == deletionDeviceIDint)
                {
                    SelectedDevice.deviceConnectionOutputIds.RemoveAt(i);
                }
            }
            
            //SelectedDevice = null;
            NotifyOfPropertyChange(() => SelectedDeviceConnections);
            NotifyOfPropertyChange(() => SelectedDevice);
            

            RefreshCanvas?.Invoke();
                
        }

        #region New Device Addition

        private string _newdeviceID { get; set; }

        bool IsAllDigits(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        public string newdeviceID
        {
            get { return _newdeviceID; }
            set
            {

                if (IsAllDigits(value))
                {
                    if(value == "") { value = "0"; }
                    //String is all numbers
                    int intValue = int.Parse(value);
                    _newdeviceID = value;
                    if (intValue < 0)
                    {
                        _newdeviceID = 0.ToString();
                    }
                    if (intValue > 65025)
                    {
                        _newdeviceID = 65025.ToString();
                    }
                }
                else
                {
                    _newdeviceID = 0.ToString();
                }
                NotifyOfPropertyChange(() => newdeviceID);
            }
        }

        public void AddNewDevice()
        {
            Console.WriteLine("Adding New Device");
            infoWindow = infoWindowShown.addDevice;

        }

        public void AddNewDeviceToDeviceList()
        {
            if (ServerManagement.checkForDuplicateDeviceIDs(int.Parse(newdeviceID)))
            {
                statusShown = true;
                statusText = "Device Exists With Same ID - Please Choose Different ID";
                statusBannerHeight = 30;
                NotifyOfPropertyChange(() => statusText);
                NotifyOfPropertyChange(() => statusShown);
                NotifyOfPropertyChange(() => statusBannerHeight);
                return;
            }
            
            DeviceModel newDevice = new DeviceModel();
            newDevice.deviceID = int.Parse(newdeviceID);
            newDevice.deviceName = "--Unsynced Device-- ("+newdeviceID+")";
            newDevice.defaultImage = 0;
            newDevice.activeImageSlot = 0;
            
            ServerManagement.addDevice(newDevice);
            RefreshCanvas.Invoke();
            infoWindow = previnfoWindow;
             statusShown = false;
            statusText = "";
            statusBannerHeight = 30;
            NotifyOfPropertyChange(() => statusText);
            NotifyOfPropertyChange(() => statusShown);
            NotifyOfPropertyChange(() => statusBannerHeight);
        }
        
        public void CancelAddingNewDevice()
        {
            infoWindow = previnfoWindow;
            statusShown = false;
            statusText = "";
            statusBannerHeight = 30;
            NotifyOfPropertyChange(() => statusText);
            NotifyOfPropertyChange(() => statusShown);
            NotifyOfPropertyChange(() => statusBannerHeight);
        }

        #endregion

        #region Device Editing

        public string editDeviceName { get; set; }


        public void editSelectedDevice()
        {
            infoWindow = infoWindowShown.editDevice;
            updateEditInfomation();
        }

        public void updateEditInfomation()
        {
            editDeviceName = SelectedDevice.deviceName;
            Console.WriteLine("Loading " + SelectedDevice.deviceName + " -> " + editDeviceName);
            NotifyOfPropertyChange(() => editDeviceName);
        }

        public void RemoveDeviceFromSystem()
        {
            ServerManagement.deleteDeviceFromSystem(SelectedDevice);
            infoWindow = infoWindowShown.none;

            NotifyOfPropertyChange(() => infoWindow);
            RefreshCanvas?.Invoke();
            updateDeviceListWindow();
            statusShown = true;
            statusText = "Routes Need Recalculating";
            statusBannerHeight = 30;
            NotifyOfPropertyChange(() => statusText);
            NotifyOfPropertyChange(() => statusShown);
            NotifyOfPropertyChange(() => statusBannerHeight);
        }

        public void UpdateEditedDevice()
        {
            SelectedDevice.deviceName = editDeviceName;
            SelectedDevice.orientationRotation = Orientation;

            updateDeviceListWindow();
            infoWindow = previnfoWindow;
            statusShown = false;
            statusText = "";
            statusBannerHeight = 30;
            NotifyOfPropertyChange(() => statusText);
            NotifyOfPropertyChange(() => statusShown);
            NotifyOfPropertyChange(() => statusBannerHeight);

            ServerResources.UDP.UDPPreformattedMessages.SetName(SelectedDevice.deviceName, SelectedDevice.deviceIP);
            ServerResources.UDP.UDPPreformattedMessages.SetOrientation(SelectedDevice.orientationRotation, SelectedDevice.deviceIP);
            
           

        }

        public void CancelEditingDevice()
        {
            infoWindow = previnfoWindow;
            statusShown = false;
            statusText = "";
            statusBannerHeight = 30;
            NotifyOfPropertyChange(() => statusText);
            NotifyOfPropertyChange(() => statusShown);
            NotifyOfPropertyChange(() => statusBannerHeight);
        }

        #endregion

       

        public void Evacuate()
        {

            if(routes != null)
            {
                ServerManagement.ShouldEvacuate();
                routes.Evacuate();
                RefreshCanvas.Invoke();
                if (ServerManagement.shouldEvacuate)
                {
                    evacButtonColour = "#A01313"; NotifyOfPropertyChange(() => evacButtonColour);
                }
                else { evacButtonColour = "#3E3E3E"; NotifyOfPropertyChange(() => evacButtonColour); }

            }
            else
            {
                statusShown = true;
                statusText = "Routes Not Precalculated";
                statusBannerHeight = 30;
                NotifyOfPropertyChange(() => statusText);
                NotifyOfPropertyChange(() => statusShown);
                NotifyOfPropertyChange(() => statusBannerHeight);
            }
        }

    }
}
