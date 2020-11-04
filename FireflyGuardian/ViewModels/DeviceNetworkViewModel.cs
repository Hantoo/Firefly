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
    //ToDo: Assigning Node As Exit
    //ToDo: Assigning Fire Or No Go Area
    //ToDo: Assigning Node As Room
    //ToDo: Add Removal Of Connections
    public delegate void NotifyCanvasRefresh();
    public delegate void NotifyNodeChange();
    public delegate void NotifyDeviceViewModelDestory();

    class DeviceNetworkViewModel : Screen
    {
        private BindableCollection<DeviceModel> _deviceList = new BindableCollection<DeviceModel>();
        public BindableCollection<DeviceModel> deviceList { get { return _deviceList; } }
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
        //public bool nodeDrag { get { return nodeDrag; } set { nodeDrag = value; if (value == true) { canvasDraga = false; } } }
        //public bool canvasDraga { get { return canvasDraga; } set { canvasDraga = value; if (value == true) { nodeDrag = false; } } }
        //public object NodeGraphViewModelView { get; set; }// = new DeviceNodeGraphViewModel();
        public DeviceNetworkViewModel()
        {

            nodeGraph = new DeviceNodeGraphViewModel();
            /*MessageBox.Show(ServerResources.ServerManagement.devices.Count.ToString());*/
            //NodeGraphViewModel = new DeviceNodeGraphViewModel();

            NotifyOfPropertyChange(() => canvasDraga);
            FireflyGuardian.Views.DeviceNodeGraphView.UserToggledCanvasDrag += toggleUserCanvasDrag;
            FireflyGuardian.Views.DeviceNodeGraphView.UserToggledNodeDrag += toggleUserNodeDrag;
            FireflyGuardian.Views.DeviceNodeGraphView.UserSelectedNode += setSelectedDeviceFromGraph;
            FireflyGuardian.ViewModels.ShellViewModel.NotfiyNewView += ViewScreen;
            FireflyGuardian.ViewModels.ShellViewModel.NotfiyDestoryView += DestoryScreen;
            

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
            
            for (int i = 0; i < ServerResources.ServerManagement.devices.Count; i++)
            {
                _deviceList.Add(ServerResources.ServerManagement.devices[i]);
            }
            NotifyOfPropertyChange(() => _deviceList);
            NotifyOfPropertyChange(() => deviceList);
            RefreshCanvas?.Invoke();
        }
        private int _selectedDeviceIndex;
        private DeviceModel _SelectedDevice;
        public BindableCollection<DeviceModel> _SelectedDeviceConnections = new BindableCollection<DeviceModel>();
        public BindableCollection<DeviceModel> SelectedDeviceConnections { get { return _SelectedDeviceConnections;  }}

        public double deviceLocationX { get; set; }
        public double deviceLocationZ { get; set; }
        public int routingType { get; set; }
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
        public void loadDevicesFromJson()
        {
            openFileDialog = new OpenFileDialog();
            // Launch OpenFileDialog by calling ShowDialog method
            openFileDialog.Filter = "JSON Files |*.json;";
            hasFile = openFileDialog.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (hasFile == true)
            {

                hasFile = false;
                string json = File.ReadAllText(openFileDialog.FileName);
                List<DeviceModel> deviceListFromFile = new List<DeviceModel>();
                deviceListFromFile = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DeviceModel>>(json);
                ServerResources.ServerManagement.devices = deviceListFromFile;

                updateDeviceListWindow();
               
            }
            
        }


        //Save the ServerManagement variable to 
        public void saveDevicesToJson()
        {
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON Files |*.json;";
            saveFileDialog.ShowDialog();
            
            json.saveDevices(saveFileDialog.FileName);
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
        }

        public void Evacuate()
        {
            if(routes != null)
            {
                routes.Evacuate();
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
