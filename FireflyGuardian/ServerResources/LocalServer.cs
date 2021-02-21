using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace FireflyGuardian.ServerResources
{
    public delegate void NotifyTickEvent();
    class LocalServer
    {

        bool serverRunning = false;
        Thread serverThread;

        DateTime nextHeartBeat;
        DateTime nextActiveImage;
        DateTime nextSecond;
        // Check if evac has happened

        //Check if routine was suppose to be active

        public void start()
        {
            serverRunning = true;
            ThreadStart childref = new ThreadStart(serverLoop);
            Console.WriteLine("[SERVER] - Created Server Thread");
            
            serverThread = new Thread(childref);
            serverThread.Start();
            
        }

        public void stop()
        {
            serverRunning = false;
        }


        
        public static event NotifyTickEvent tickComplete;
        public void serverLoop()
        {

            Console.WriteLine("[SERVER] - Server Running");
            nextHeartBeat = DateTime.Now;
            nextActiveImage = DateTime.Now;
           
            while (serverRunning)
            {
               
                checkEvacuate();
                //Every 10 Seconds Do:
                ServerManagement.nextHeartBeatCheck_reference = Convert.ToInt32((nextHeartBeat - DateTime.Now).TotalSeconds);
                ServerManagement.nextGlobalUpdate_reference = Convert.ToInt32((nextActiveImage - DateTime.Now).TotalSeconds);
                if (DateTime.Compare(nextHeartBeat, DateTime.Now) <= 0 && !ServerManagement.shouldEvacuate)
                {
                    Console.WriteLine("[LOCAL SERVER] HeartBeat");
                    heartbeat();
                }
                //Every Minute Do:
                if (DateTime.Compare(nextActiveImage, DateTime.Now) <= 0 && !ServerManagement.shouldEvacuate)
                {
                    Console.WriteLine("[LOCAL SERVER] Active Image");
                    activeImage();
                    checkRoutinesTimes();
                }
                if (DateTime.Compare(nextSecond, DateTime.Now) <= 0)
                {
                    OnProcessCompleted();
                    nextSecond = DateTime.Now.AddSeconds(1.0);
                }
                
            }
        }

        protected virtual void OnProcessCompleted()
        {
            tickComplete?.Invoke();
            
        }

        public void activeImage()
        {
            nextActiveImage = DateTime.Now;
            nextActiveImage = nextActiveImage.AddMinutes(1);
            for (int i = 0; i < ServerManagement.devices.Count; i++)
            {
                //Will send out the image to any devices not being controlled via a routine
                if (!ServerManagement.devices[i].isRunningRoutine)
                {

                    byte[] msg = { 0xFF, 0x03, 0x01, (byte)ServerManagement.devices[i].activeImageSlot };
                    ServerManagement.udpServer.UDPSend(msg, ServerManagement.devices[i].deviceIP);
                }
            }
        }

        //Evacuation will keep looping until evac is no longer needed.
        public void checkEvacuate() {
            if (ServerManagement.shouldEvacuate)
            {
                while (ServerManagement.shouldEvacuate)
                {
                    
                    for (int i = 0; i < ServerManagement.devices.Count; i++) {

                        byte[] msg = { 0xFF, 0x03, 0x01, (byte)ServerManagement.devices[i].activeImageSlot};
                        ServerManagement.udpServer.UDPSend(msg, ServerManagement.devices[i].deviceIP);
                    }
                    OnProcessCompleted();
                    Thread.Sleep(1000); //Sleep for 1 second, then loop again
                }
            }
        }

        public void heartbeat()
        {
            Console.WriteLine("[LOCAL SERVER] HeatBeat Pulse Out");
            Byte[] message = { 0xff, 0x02 };
            IPAddress[] addresses = ServerManagement.udpServer.GetBroadCastIP();
            for (int i = 0; i < addresses.Length; i++)
            {
                string ipAddressString = addresses[i].ToString();
                Console.WriteLine("Broadcast IP address: {0}", ipAddressString);
                ServerManagement.udpServer.UDPSend(message, ipAddressString);
            }
            nextHeartBeat = DateTime.Now;
            nextHeartBeat = nextHeartBeat.AddSeconds(10);
        }

        //This will go through the routines and check them against the time of day. If they should be running it will set the isRunning
        //Variable to true.
        public void checkRoutinesTimes()
        {
          
            for (int i = 0; i < ServerManagement.routines.Count; i++)
            {
                if (ServerManagement.routines[i].routineActive)
                {
                   
                    for (int j = 0; j < ServerManagement.routines[i].rountineTiming.Count; j++)
                    {
                        int dayNum = (int)(DateTime.Now.DayOfWeek + 6) % 7;
                        if (ServerManagement.routines[i].rountineTiming[j].daysActive[dayNum]) //If routine day was set to active on this day then
                        {
                            //Check time
                            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, ServerManagement.routines[i].rountineTiming[j].startHour, ServerManagement.routines[i].rountineTiming[j].startMin, 0);
                            DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, ServerManagement.routines[i].rountineTiming[j].endHour, ServerManagement.routines[i].rountineTiming[j].endMin, 0);
                            DateTime.Compare(DateTime.Now, startDate);
                            if (DateTime.Compare(DateTime.Now, startDate) >= 0 && DateTime.Compare(DateTime.Now, endDate) < 0)
                            {

                                if (!ServerManagement.routines[i].isRunning)
                                {
                                   
                                    ServerManagement.routines[i].isRunning = true;
                                    //If routine should be playing then start a new thread for the routine so that it can run without affecting the program
                                    ServerManagement.routines[i].startRoutine();
                                }
                                break;
                            }
                            else
                            {
                                if (ServerManagement.routines[i].isRunning)
                                {
                                    ServerManagement.routines[i].isRunning = false;
                                    for (int p = 0; p < ServerManagement.routines[i].deviceIDsToRun.Count; p++)
                                    {
                                        for (int k = 0; k < ServerResources.ServerManagement.devices.Count; k++)
                                        {
                                            if (ServerResources.ServerManagement.devices[k].deviceID == ServerManagement.routines[i].deviceIDsToRun[p])
                                            {
                                                
                                                ServerResources.ServerManagement.devices[k].activeImageSlot = ServerResources.ServerManagement.devices[k].defaultImage;
                                            }
                                        }
                                    }
                                                
                                }
                            }

                        }
                    }
                }
            }
        }

     

    }
}
