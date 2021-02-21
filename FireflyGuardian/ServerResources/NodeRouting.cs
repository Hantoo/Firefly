using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;
using FireflyGuardian.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FireflyGuardian.ServerResources
{
    class NodeRouting
    {
        private Graph<int, string> graph;
        private List<int> exitNodeIds;
        private List<DeviceModel> locallist_devices;
        const double Rad2Deg = 180.0 / Math.PI; 
        private int highestDeviceID = 0;

        public NodeRouting()
        {
            graph = new Graph<int, string>();
            exitNodeIds = new List<int>();
            locallist_devices = new List<DeviceModel>();
        }

        //Goes through every node and adds it to the graph.
        //Then goes through every node and looks at the device connections.
        //It calculates the distance between the node and it's device connection
        //It connects the node and the device connection with the caluclated distance (as the bird flies)
        public void makeGraphFromDeviceList(List<DeviceModel> devices)
        {
           if(devices.Count == 0) { return; }
            exitNodeIds.Clear();
            locallist_devices.Clear();
            locallist_devices = devices;
            graph = new Graph<int, string>();
            int[] deviceIDs = new int[devices.Count];
            for (int i = 0; i < devices.Count; i++)
            {
                deviceIDs[i] = devices[i].deviceID;
            }
            highestDeviceID = deviceIDs.Max();
            //for (int i = 0; i < devices.Count; i++)
            //{
            //    Console.WriteLine("Graphing: " + devices[i].deviceID);
            //    graph.AddNode(devices[i].deviceID);
            //    devices[i].exitRoutings.Clear();
            //    //If node is a exit node then add to exit node list
            //    if (devices[i].isExit)
            //    {
            //        exitNodeIds.Add(devices[i].deviceID);
            //        devices[i].activeImageSlot = 10;
            //        devices[i].emergecnyImage = 10;
            //    }

            //}
            for (int i = 0; i < devices.Count; i++)
            {
                devices[i].exitRoutings.Clear();
                //If node is a exit node then add to exit node list
                if (devices[i].isExit)
                {
                    exitNodeIds.Add(devices[i].deviceID);
                    devices[i].activeImageSlot = 10;
                    devices[i].emergecnyImage = 10;
                }
            }
            for (int i = 0; i < highestDeviceID; i++)
            {
                Console.WriteLine("Graphing: " + i);
                graph.AddNode(i);
            }
            for (int i = 0; i < devices.Count; i++)
            {
                for (int j = 0; j < devices[i].deviceConnectionOutputIds.Count; j++)
                {
                    double connectionX = 0;
                    double connectionZ = 0;
                    for (int k = 0; k < devices.Count; k++)
                    {
                        if (devices[k].deviceID == devices[i].deviceConnectionOutputIds[j])
                        {
                            connectionX = devices[k].deviceXZLocationOnGrid.Item1;
                            connectionZ = devices[k].deviceXZLocationOnGrid.Item2;
                            break;
                        }
                    }

                    double calculatedDistanceX = devices[i].deviceXZLocationOnGrid.Item1 - connectionX;
                    double calculatedDistanceZ = devices[i].deviceXZLocationOnGrid.Item2 - connectionZ;

                    double distance = Math.Sqrt((Math.Pow(calculatedDistanceX, 2) + Math.Pow(calculatedDistanceZ, 2)));

                    //Graph connection is only one way, so reverse graph path
                    if (!devices[i].flagEmergencyAtNode && !getInfomationAboutDevice(devices[i].deviceConnectionOutputIds[j]).flagEmergencyAtNode)
                    {
                        //Connection To
                        Console.WriteLine(devices[i].deviceID + " = " + devices[i].deviceConnectionOutputIds[j]);
                        graph.Connect((uint)devices[i].deviceID, (uint)devices[i].deviceConnectionOutputIds[j], (Convert.ToInt32(distance)), "some custom information in edge");
                        // Return Connection 
                        Console.WriteLine(devices[i].deviceConnectionOutputIds[j] + " = " + devices[i].deviceID);
                        graph.Connect((uint)devices[i].deviceConnectionOutputIds[j], (uint)devices[i].deviceID, (Convert.ToInt32(distance)), "some custom information in edge");
                    }
                }
            }
            
            calculateRoutesForAllExitsForAllNodes();

        }
       

        public DeviceModel getInfomationAboutDevice(int idNum)
        {
            for (int i = 0; i < locallist_devices.Count; i++)
            {
                if(locallist_devices[i].deviceID == idNum)
                {
                    return locallist_devices[i];
                }
            }
            return null;           
        }

        public void calculateRoutesForAllExitsForAllNodes()
        {
            long startTime = utils.GetUnixTimestampMillis();
            
            List<DeviceModel> templist = new List<DeviceModel>(locallist_devices);

            Console.WriteLine("Node Count: " + graph.NodesCount);
            IEnumerator<INode<int, string>> inodes = graph.GetEnumerator();
            int[] KeyValueArray = new int[graph.NodesCount];
            int itteration = 0;
            while (inodes.MoveNext())
            {
                KeyValueArray[itteration] = inodes.Current.Item;
                itteration++;
                Console.WriteLine("Key: " + inodes.Current.Key + ", Item: " + inodes.Current.Item);
            }

            //for every node in the device list
            for (int i = 0; i < locallist_devices.Count; i++)
            {
                //Get Random Index Number to choose random node to analyse
                //Using Random should hopefully mean that we can map nodes quicker
                Random rnd = new Random();
                int randNum = rnd.Next(0, templist.Count);
                DeviceModel node = templist[randNum];
                templist.RemoveAt(randNum);
                
                //If the Node is an exit node, don't worry about calculating the route
                bool isExitNode = false;
                for (int j = 0; j < exitNodeIds.Count; j++) {
                    if (node.deviceID == exitNodeIds[j])
                    {
                        isExitNode = true;
                        
                        break;
                    }
                }
                if (isExitNode)
                {
                    
                    continue;
                }
                Console.WriteLine("Device: " + node.deviceID);
              
                //Node is not an exit node so calculate the route to all exit notdes
                for (int j = 0; j < exitNodeIds.Count; j++)
                {
                    Console.WriteLine((uint)node.deviceID + " : " + (uint)exitNodeIds[j] + " - " + node.deviceConnectionOutputIds.Count);
                    //if(node.deviceConnectionOutputIds.Count == 0) { continue; }
                    ShortestPathResult result;
                    try
                    {
                        
                        //result = graph.Dijkstra(fromKeyIndex, toKeyIndex);
                        result = graph.Dijkstra((uint)node.deviceID, (uint)exitNodeIds[j]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("=====");
                        Console.WriteLine("Dijkstrra Error: " + e.Message);
                        Console.WriteLine("=====");
                        continue;
                    }
                    List<uint> Route = new List<uint>();
                    Console.WriteLine("Route");
                    foreach (uint nodeID in result.GetPath())
                    {
                        Route.Add(nodeID);
                        Console.Write(nodeID+ " -> ");
                    }
                    //Go through every node within the route
                    for (int m = 0; m < Route.Count; m++) {
                        uint nodeID = Route[m];
                        //Go throug every node
                        //Check if node is an exit node - if, so skip
                        bool shouldSkip = false;
                        for (int n = 0; n < exitNodeIds.Count; n++)
                        {
                            if (nodeID == exitNodeIds[n])
                            {
                                
                                shouldSkip = true;
                                
                                break;
                            }
                        }
                        if (shouldSkip)
                        {
                            Console.WriteLine("Exit Node Detected: Skipping Route Calculation");
                            continue;
                        }

                        for (int k = 0; k < locallist_devices.Count; k++)
                        {
                            //If the node id matches the current node being examined in the route then
                            if (locallist_devices[k].deviceID == nodeID)
                            {
                                //If node has been found in list, search exitroutings to see if route already exits, if not add definition
                                bool routingAlreadyDefined = false;
                                for (int l = 0; l < locallist_devices[k].exitRoutings.Count; l++) {
                                    
                                    if (locallist_devices[k].exitRoutings[l].TargetExitNode == exitNodeIds[j])
                                    {
                                        //Routing Already Present
                                        routingAlreadyDefined = true;
                                        
                                        break;
                                    }
                                }
                                //Routing not defined within Node, add definition
                                if (routingAlreadyDefined == false)
                                {
                                    DeviceModel.exitRouting routing = new DeviceModel.exitRouting();
                                    routing.TargetExitNode = exitNodeIds[j];
                                    
                                    List<int> fullPath = new List<int>();
                                    if (m < Route.Count - 1) {
                                        routing.nextNodeID = (int)Route[m + 1];
                                        for(int o = 0, p = m; o < (Route.Count - 1 - m); o++, p++)
                                        {
                                            fullPath.Add((int)Route[p + 1]);
                                        }
                                       

                                        //Search Through list until have XZ calcs on both this node and next node
                                        (double, double) device1XZ = (0,0);
                                        (double, double) device2XZ = (0,0);
                                        for (int o = 0; o < locallist_devices.Count; o++)
                                        {
                                            if(locallist_devices[o].deviceID == Route[m]) { device1XZ.Item1 = locallist_devices[o].deviceXZLocationOnGrid.Item1; device1XZ.Item2 = locallist_devices[o].deviceXZLocationOnGrid.Item2; }
                                            if(locallist_devices[o].deviceID == Route[m+1]) { device2XZ.Item1 = locallist_devices[o].deviceXZLocationOnGrid.Item1; device2XZ.Item2 = locallist_devices[o].deviceXZLocationOnGrid.Item2; }
                                        }

                                        double angle = Math.Atan2(device2XZ.Item2 - device1XZ.Item2, device2XZ.Item1 - device1XZ.Item1) * Rad2Deg;
                                        if(angle >= 0) //Upwards
                                        {
                                            if(angle < 22.5 && angle >= 0 ) { routing.directionToNextNode = 5; } //Right
                                            if(angle < 67.5 && angle >= 22.5 ) { routing.directionToNextNode = 4; } // Up / Right
                                            if(angle < 112.5 && angle >= 67.5 ) { routing.directionToNextNode = 3; } // Up 
                                            if(angle < 157.5 && angle >= 112.5 ) { routing.directionToNextNode = 2; } // Up / Left
                                            if(angle <= 180 && angle >= 157.5 ) { routing.directionToNextNode = 1; } // Left 
                                        }
                                        if (angle < 0) //Down
                                        {
                                            if (angle < 0 && angle >= -22.5) { routing.directionToNextNode = 5; } //Right
                                            if (angle < -22.5 && angle >= -67.5) { routing.directionToNextNode = 6; } // Down / Right
                                            if (angle < -67.5 && angle >= -112.5) { routing.directionToNextNode = 7; } // Down
                                            if (angle < -112.5 && angle >= -157.5) { routing.directionToNextNode = 8; } // Down / Left
                                            if (angle <= -157.5 && angle >= -180) { routing.directionToNextNode = 1; } // Left 
                                        }
                                        ShortestPathResult DistanceToExit = graph.Dijkstra((uint)locallist_devices[k].deviceID, (uint)exitNodeIds[j]);
                                        routing.distanceToTargetExitNode = DistanceToExit.Distance;
                                    }


                                    routing.fullPath = fullPath;
                                    locallist_devices[k].exitRoutings.Add(routing);
                                }                               
                            }
                        }
                    }
                }
            }



            for (int i = 0; i < locallist_devices.Count; i++)
            {
                int smallestDistance = 900000;
                int smallestDistanceIndex = -1;

                for (int j = 0; j < locallist_devices[i].exitRoutings.Count; j++)
                {

                    if (locallist_devices[i].exitRoutings[j].distanceToTargetExitNode < smallestDistance)
                    {
                        smallestDistance = locallist_devices[i].exitRoutings[j].distanceToTargetExitNode;
                        smallestDistanceIndex = j;
                    }

                }

                if (smallestDistanceIndex == -1)
                {
                    locallist_devices[i].emergecnyImage = 0;
                }
                else
                {
                    locallist_devices[i].emergecnyImage = locallist_devices[i].exitRoutings[smallestDistanceIndex].directionToNextNode;
                }
                if (locallist_devices[i].flagEmergencyAtNode)
                {
                    locallist_devices[i].emergecnyImage = 9;
                }
                for (int k = 0; k < locallist_devices.Count; k++)
                {

                    for (int n = 0; n < exitNodeIds.Count; n++)
                    {

                        if (locallist_devices[k].deviceID == exitNodeIds[n])
                        {
                            locallist_devices[k].emergecnyImage = 10;

                        }
                    }
                }

                //whichever exit that is, set the current image to be the direction to the node.
            }
        

        ServerResources.ServerManagement.devices = locallist_devices;


        }


        public void Evacuate()
        {
            //Go Through all nodes and check for the shortest distance to the exit
            for (int i = 0; i < locallist_devices.Count; i++)
            {
                int smallestDistance = 900000;
                int smallestDistanceIndex = -1;

                for (int j = 0; j < locallist_devices[i].exitRoutings.Count; j++)
                {

                    if(locallist_devices[i].exitRoutings[j].distanceToTargetExitNode < smallestDistance)
                    {
                        smallestDistance = locallist_devices[i].exitRoutings[j].distanceToTargetExitNode;
                        smallestDistanceIndex = j;
                    }

                }

                if(smallestDistanceIndex == -1)
                {
                    locallist_devices[i].activeImageSlot = 0;
                }
                else {
                    locallist_devices[i].activeImageSlot = locallist_devices[i].exitRoutings[smallestDistanceIndex].directionToNextNode;
                }
                if (locallist_devices[i].flagEmergencyAtNode)
                {
                    locallist_devices[i].activeImageSlot = 9;
                }
                for (int k = 0; k < locallist_devices.Count; k++)
                {

                    for (int n = 0; n < exitNodeIds.Count; n++)
                    {

                        if (locallist_devices[k].deviceID == exitNodeIds[n])
                        {
                            locallist_devices[k].activeImageSlot = 10;

                        }
                    }
                }

                //whichever exit that is, set the current image to be the direction to the node.
            }
        }

    }
}
