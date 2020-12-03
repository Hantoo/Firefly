using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Newtonsoft.Json;

namespace FireflyGuardian.Models
{
    class RoutineModel
    {
        public class routineSlot
        {

            public int routineIndex { get; set; }
            public int routineImageSlot { get; set; }

            public int routineDisplayTime { get; set; }
            public string routineSlotName { get; set; }

            [JsonIgnore]
            public BitmapImage runtimeImage { get; set; }
            public routineSlot()
            {
                this.routineSlotName = "Slot " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.Millisecond;
                this.routineImageSlot = 0;
            }

        }
        public class routineTimeSlot
        {
            public string timeslotName { get; set; }
            public DateTime startTime { get; set; }
            public int startHour { get { return startTime.Hour; } set { startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, value, startTime.Minute, 0); } }
            public int endHour { get { return endTime.Hour; } set { endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, value, endTime.Minute, 0); } }
            public int startMin { get { return startTime.Minute; } set { startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, startTime.Hour, value, 0);  } }
            public int endMin { get { return endTime.Minute; } set { endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, endTime.Hour, value, 0);  } }
            public string startTimeString { get { return startTime.ToShortTimeString(); } }
            public string endTimeString { get { return endTime.ToShortTimeString(); } }
            public DateTime endTime { get; set; }
            private int _routineScheduleMode { get; set; }//0 Everyday, 1 WeekDays, 2 Weekends, 3 CertainDays
            public int routineScheduleMode { get { return _routineScheduleMode; } set { _routineScheduleMode = value;
                    switch (_routineScheduleMode)
                    {
                        case 0:
                            daysActive.SetValue(true, 0);
                            daysActive.SetValue(true, 1);
                            daysActive.SetValue(true, 2);
                            daysActive.SetValue(true, 3);
                            daysActive.SetValue(true, 4);
                            daysActive.SetValue(true, 5);
                            daysActive.SetValue(true, 6);
                            break;
                        case 1:
                            daysActive.SetValue(true, 0);
                            daysActive.SetValue(true, 1);
                            daysActive.SetValue(true, 2);
                            daysActive.SetValue(true, 3);
                            daysActive.SetValue(true, 4);
                            daysActive.SetValue(false, 5);
                            daysActive.SetValue(false, 6);
                            break;
                        case 2:
                            daysActive.SetValue(false, 0);
                            daysActive.SetValue(false, 1);
                            daysActive.SetValue(false, 2);
                            daysActive.SetValue(false, 3);
                            daysActive.SetValue(false, 4);
                            daysActive.SetValue(true, 5);
                            daysActive.SetValue(true, 6);
                            break;
                    }
                } }//0 Everyday, 1 WeekDays, 2 Weekends, 3 CertainDays

            public bool[] daysActive { get; set; }
            public string days { get; set; }

            public routineTimeSlot()
            {
                daysActive = new bool[7];
                timeslotName = "Routine " +DateTime.Now.ToLongDateString() +" "+ DateTime.Now.ToLongTimeString() + " "+DateTime.Now.Millisecond;
                
            }

           

        }
        public int timeslotsidx { get; set; }
        public string routineName { get; set; }
        public string routineIcon { get; set; }
        public DateTime routineCreationDate { get; set; }
        public string routineIconHexColour { get; set; }
        public int routineScheduleIndex { get
            {
                if (rountineTiming != null && rountineTiming.Count > 0)
                {
                    return rountineTiming[0].routineScheduleMode;
                }
                else { return 0; }
            } set { foreach (routineTimeSlot slot in rountineTiming) { slot.routineScheduleMode = value; } } } //0 Everyday, 1 WeekDays, 2 Weekends, 3 CertainDays
        public string routineSlotCount { get { return routine.Count().ToString();  } }
        public List<routineSlot> routine = new List<routineSlot>();
        public List<routineTimeSlot> rountineTiming = new List<routineTimeSlot>();
        public bool routineActive { get; set; }
        [JsonIgnore]
        public bool suggestDelete { get; set; }
        [JsonIgnore]
        public bool isRunning { get; set; }
        [JsonIgnore]
        public Thread routineThread;
        public List<int> deviceIDsToRun = new List<int>();
        
        public RoutineModel()
        {
            ThreadStart childref = new ThreadStart(threadRoutineLoop);
            Console.WriteLine("[ROUTINE] - Created ROUTINE Thread");
            routineThread = new Thread(childref);
        }

        public void startRoutine()
        {

            if(!routineThread.IsAlive) 
            {
                ThreadStart childref = new ThreadStart(threadRoutineLoop);
                routineThread = new Thread(childref);
                routineThread.Start();
            }
        }

        public void threadRoutineLoop()
        {
            while (isRunning && routineActive)
            {
                for(int i =0; i < routine.Count; i++)
                {
                    int imageNum = routine[i].routineImageSlot;
                    for (int j = 0; j < deviceIDsToRun.Count; j++)
                    {
                        for(int k = 0; k < ServerResources.ServerManagement.devices.Count; k++)
                        {
                            if(ServerResources.ServerManagement.devices[k].deviceID == deviceIDsToRun[j])
                            {
                                ServerResources.ServerManagement.devices[k].activeImageSlot = imageNum;
                                byte[] msg = { 0xFF, 0X03, 0X01, (byte)imageNum };
                                //Console.WriteLine("Output to : "+ ServerResources.ServerManagement.devices[k].deviceID + ", "+ ServerResources.ServerManagement.devices[k].deviceIP);
                                ServerResources.ServerManagement.udpServer.UDPSend(msg, ServerResources.ServerManagement.devices[k].deviceIP);
                                ServerResources.ServerManagement.devices[k].activeImageSlot = imageNum;
                                ServerResources.ServerManagement.devices[k].isRunningRoutine = true;
                            }
                        }
                    }
                    Console.WriteLine("Output: Image ID: " + imageNum + " - Sleeping for: " + routine[i].routineDisplayTime * 1000);
                    Thread.Sleep(routine[i].routineDisplayTime * 1000);
                }
            }
            isRunning = false;
            for (int j = 0; j < deviceIDsToRun.Count; j++)
            {
                for (int k = 0; k < ServerResources.ServerManagement.devices.Count; k++)
                {
                    if (ServerResources.ServerManagement.devices[k].deviceID == deviceIDsToRun[j])
                    {
                        byte[] msg = { 0xFF, 0X03, 0X01, (byte)ServerResources.ServerManagement.devices[k].defaultImage};
                        ServerResources.ServerManagement.devices[k].activeImageSlot = ServerResources.ServerManagement.devices[k].defaultImage;
                        ServerResources.ServerManagement.udpServer.UDPSend(msg, ServerResources.ServerManagement.devices[k].deviceIP);
                        ServerResources.ServerManagement.devices[k].isRunningRoutine = false;
                    }
                }
            }

        }

      
    }
}
