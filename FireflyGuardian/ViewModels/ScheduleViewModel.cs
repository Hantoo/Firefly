using Caliburn.Micro;
using FireflyGuardian.Models;
using FireflyGuardian.ServerResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using System.Windows.Data;
using System.ComponentModel;

namespace FireflyGuardian.ViewModels
{

    public class DataContextSpy
    : Freezable // Enable ElementName and DataContext bindings
    {
        public DataContextSpy()
        {
            // This binding allows the spy to inherit a DataContext.
            BindingOperations.SetBinding(this, DataContextProperty, new Binding());

            this.IsSynchronizedWithCurrentItem = true;
        }

        /// <summary>
        /// Gets/sets whether the spy will return the CurrentItem of the 
        /// ICollectionView that wraps the data context, assuming it is
        /// a collection of some sort. If the data context is not a 
        /// collection, this property has no effect. 
        /// The default value is true.
        /// </summary>
        public bool IsSynchronizedWithCurrentItem { get; set; }

        public object DataContext
        {
            get { return (object)GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        // Borrow the DataContext dependency property from FrameworkElement.
        public static readonly DependencyProperty DataContextProperty =
            FrameworkElement.DataContextProperty.AddOwner(
            typeof(DataContextSpy),
            new PropertyMetadata(null, null, OnCoerceDataContext));

        static object OnCoerceDataContext(DependencyObject depObj, object value)
        {
            DataContextSpy spy = depObj as DataContextSpy;
            if (spy == null)
                return value;

            if (spy.IsSynchronizedWithCurrentItem)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(value);
                if (view != null)
                    return view.CurrentItem;
            }

            return value;
        }

        protected override Freezable CreateInstanceCore()
        {
            // We are required to override this abstract method.
            throw new NotImplementedException();
        }
    }

    class ScheduleViewModel : Screen
    {

        public bool showStatus { get; set; }
        public string statusMessage { get; set; }
        public FireflyGuardian.ViewModels.statusState statusStateIndicator { get; set; }
        public bool hasSelectedRoutine { get; set; }
        private BindableCollection<RoutineModel> _routines = new BindableCollection<RoutineModel>();
        public BindableCollection<RoutineModel> routines { get { return _routines; } set { _routines = value; NotifyOfPropertyChange(() => routines); } }
        //public BindableCollection<RoutineModel> routines { get; set; }
        //private RoutineModel _SelectedRoutine { get; set; }
       // public RoutineModel SelectedRoutine { get { return _SelectedRoutine; } set { _SelectedRoutine = value; Console.WriteLine("Changed Selected"); NotifyOfPropertyChange(() => SelectedRoutine); } }
        private RoutineModel _selectedRoutine { get; set; }
        public RoutineModel SelectedRoutine { get { return _selectedRoutine; } set { _selectedRoutine = value; hasSelectedRoutine = false; if (_selectedRoutine != null) { hasSelectedRoutine = true; timeslots = new BindableCollection<RoutineModel.routineTimeSlot>(_selectedRoutine.rountineTiming); mediaslots = new BindableCollection<RoutineModel.routineSlot>(_selectedRoutine.routine); selectedRoutineDeviceIds = new BindableCollection<int>(_selectedRoutine.deviceIDsToRun); } NotifyOfPropertyChange(() => selectedRoutineDeviceIds); NotifyOfPropertyChange(() => hasSelectedRoutine); NotifyOfPropertyChange(() => mediaslots); NotifyOfPropertyChange(() => SelectedRoutine); NotifyOfPropertyChange(() => timeslots); } }
        private RoutineModel _HoverOverRoutine { get; set; }
        public RoutineModel HoverOverRoutine { get { return _HoverOverRoutine; } set { _HoverOverRoutine = value; NotifyOfPropertyChange(() => HoverOverRoutine); } }
        public BindableCollection<DeviceModel> localdevices { get; set; }
        public BindableCollection<int> selectedRoutineDeviceIds { get; set; }
        public DeviceModel selectedDeviceToRoutine { get; set; }
        public BindableCollection<RoutineModel.routineTimeSlot> timeslots { get; set; }
        private BindableCollection<RoutineModel.routineSlot> _mediaslots { get; set; }
        public BindableCollection<RoutineModel.routineSlot> mediaslots { get { return _mediaslots; } set
            {
                if (_mediaslots != null)
                {
                    for (int i = 0; i < _mediaslots.Count; i++)
                    {
                        _mediaslots[i].runtimeImage = null;
                    }
                }

                _mediaslots = value;
        
                for (int i = 0; i < _mediaslots.Count; i++)
                {
                    _mediaslots[i].runtimeImage = localmediaslots[_mediaslots[i].routineImageSlot].image;
                    _mediaslots[i].routineSlotName = ServerManagement.settings.slotNames[i];
                }
            }
        }
        
      
        public static BindableCollection<MediaSlotModel> localmediaslots { get; set; }
  
        public ScheduleViewModel()
        {
            localdevices = new BindableCollection<DeviceModel>(ServerManagement.devices);
            NotifyOfPropertyChange(() => localdevices);
            FireflyGuardian.Views.ScheduleView.DeletionEvent += TriggerSuggestedDelete;
            FireflyGuardian.Views.ScheduleView.HoverChanged += updateHover;
            FireflyGuardian.Views.ScheduleView.TimeSlotHoverChanged += TimeSlotHoverUpdate;
            FireflyGuardian.Views.ScheduleView.DateTimeRefreshEvent += updateTimeSlotsForSelectedRoutine;
            routines = new BindableCollection<RoutineModel>(ServerManagement.routines);
            //NotifyOfPropertyChange(() => routines);
            FireflyGuardian.ViewModels.ShellViewModel.NotfiyNewView += ViewScreen;
            FireflyGuardian.ViewModels.ShellViewModel.NotfiyDestoryView += DestoryScreen;
            if (ServerManagement.mediaSlots != null)
            {
                localmediaslots = new BindableCollection<MediaSlotModel>(ServerManagement.mediaSlots);
            }
        }

        public void AddSelectedDeviceToRoutine()
        {
            if (selectedDeviceToRoutine != null && SelectedRoutine != null) {
                SelectedRoutine.deviceIDsToRun.Add(selectedDeviceToRoutine.deviceID);
                SelectedRoutine = SelectedRoutine;
                NotifyOfPropertyChange(() => SelectedRoutine);
            }
            NotifyOfPropertyChange(() => SelectedRoutine);
        }

        public void RemoveDeviceFromRoutine(object sender, MouseButtonEventArgs e)
        {
            //var btn = sender as Button;
            if (e.ChangedButton == MouseButton.Left && sender is Button btn)
            {
                
                for(int i =0; i < SelectedRoutine.deviceIDsToRun.Count; i++)
                {
                    if(SelectedRoutine.deviceIDsToRun[i] == int.Parse(btn.Tag.ToString()))
                    {
                        Console.WriteLine("Removing ID " + btn.Tag.ToString() + "From Routine");
                        SelectedRoutine.deviceIDsToRun.RemoveAt(i);
                        SelectedRoutine = SelectedRoutine;
                        NotifyOfPropertyChange(() => SelectedRoutine);
                        
                        
                    }
                }
            }
            e.Handled = true;
        }


        public void ViewScreen()
        {
           /* if (FireflyGuardian.ViewModels.ShellViewModel.activePageType == this.GetType())
            {
                FireflyGuardian.Views.ScheduleView.DeletionEvent += TriggerSuggestedDelete;
            }*/
        }
        public void DestoryScreen()
        {
            if (FireflyGuardian.ViewModels.ShellViewModel.activePageType == this.GetType())
            {
                FireflyGuardian.ViewModels.ShellViewModel.NotfiyNewView -= ViewScreen;
                FireflyGuardian.ViewModels.ShellViewModel.NotfiyDestoryView -= DestoryScreen;
                FireflyGuardian.Views.ScheduleView.DeletionEvent -= TriggerSuggestedDelete;
                FireflyGuardian.Views.ScheduleView.HoverChanged -= updateHover;
                FireflyGuardian.Views.ScheduleView.TimeSlotHoverChanged -= TimeSlotHoverUpdate;
                FireflyGuardian.Views.ScheduleView.DateTimeRefreshEvent -= updateTimeSlotsForSelectedRoutine;
            }
        }

        #region statusBanner

        public void changeStatusBackGround(statusState state)
        {
            statusStateIndicator = state;
            NotifyOfPropertyChange(() => statusStateIndicator);

        }

        public async void ShowStatus(string message, int timeShown, statusState state)
        {
            changeStatusBackGround(state);
            await PutTaskDelay(message, timeShown);
        }

        async Task PutTaskDelay(string msg, int timeout)
        {
            showStatus = true;
            NotifyOfPropertyChange(() => showStatus);
            statusMessage = msg;
            NotifyOfPropertyChange(() => statusMessage);
            await Task.Delay(timeout);
            showStatus = false;
            NotifyOfPropertyChange(() => showStatus);
            await Task.Delay(500);
            statusMessage = "";
            NotifyOfPropertyChange(() => statusMessage);
        }

        #endregion

        public void updateHover(object Sender)
        {
            Border sender = Sender as Border;
            for (int i = 0; i < routines.Count; i++) {
                if(routines[i].routineName == sender.Tag.ToString())
                {
                    HoverOverRoutine = routines[i];
                }
            }
            
        }

        public int hoverOverTimeSlotIndex = 0;
        public void TimeSlotHoverUpdate(object Sender)
        {
            if (timeslots != null)
            {
                StackPanel sender = Sender as StackPanel;
               
                for (int i = 0; i < timeslots.Count; i++)
                {
                  
                    if (timeslots[i].timeslotName == sender.Tag.ToString())
                    {
                        hoverOverTimeSlotIndex = i;
                    }
                }
            }
            
        }

        public void DeleteTimeSlot()
        {
            if(timeslots != null)
            {
                
                SelectedRoutine.rountineTiming.RemoveAt(hoverOverTimeSlotIndex);
                NotifyOfPropertyChange(() => SelectedRoutine);
                updateTimeSlotsForSelectedRoutine();
            }
        }



        public void updateTimeSlotsForSelectedRoutine()
        {
         if (timeslots != null && _selectedRoutine != null)
            {
               
                timeslots = new BindableCollection<RoutineModel.routineTimeSlot>(_selectedRoutine.rountineTiming);
                NotifyOfPropertyChange(() => timeslots);
               
            }
        }

        public void AddRoutine()
        {
            
            RoutineModel routine = new RoutineModel();
            routine.routineCreationDate = DateTime.Now;
            routine.routineIcon = "\uE953";
            routine.routineIconHexColour = "#fff";
            routine.routineName = "Routine " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.Millisecond;
            
            ServerManagement.routines.Add(routine);
            routines = new BindableCollection<RoutineModel>(ServerManagement.routines);

            NotifyOfPropertyChange(() => routines);
        }

        public void refreshList()
        {
            //routines.Clear();
            //NotifyOfPropertyChange(() => routines);
            routines = new BindableCollection<RoutineModel>(ServerManagement.routines);
            NotifyOfPropertyChange(() => routines);
        }


        public void ConfirmDelete()
        {
            SelectedRoutine = null;
            timeslots = null;
            NotifyOfPropertyChange(() => SelectedRoutine);
            NotifyOfPropertyChange(() => timeslots);
            for (int i = 0; i < ServerManagement.routines.Count; i++) {
                if(ServerManagement.routines[i].routineName == HoverOverRoutine.routineName)
                {
                    ServerManagement.routines.RemoveAt(i);
                }
            }
           
            refreshList();
        }
        public void CancelDelete()
        {
           
            HoverOverRoutine.suggestDelete = false;
            NotifyOfPropertyChange(() => SelectedRoutine);
            refreshList();
        }
        public void TriggerSuggestedDelete()
        {
            if (SelectedRoutine != null)
            {
                
                //SelectedRoutine.suggestDelete = true;
                //NotifyOfPropertyChange(() => SelectedRoutine);
                for (int i = 0; i < routines.Count; i++)
                {
                    if (routines[i].routineName == HoverOverRoutine.routineName)
                    {
                        ServerManagement.routines[i].suggestDelete = true;
                       // NotifyOfPropertyChange(() => routines);

                    }
                }
                

            }
            else
            {
                Console.WriteLine("No Selected Routine");
            }
            refreshList();
            //SelectedRoutine.suggestDelete = true;
            //NotifyOfPropertyChange(() => SelectedRoutine);
            //NotifyOfPropertyChange(() => routines);
        }
        

        public void AddTimeSlot()
        {
            if (SelectedRoutine != null)
            {
                RoutineModel.routineTimeSlot timeSlot = new RoutineModel.routineTimeSlot();
                timeSlot.startTime = DateTime.Now;
                timeSlot.endTime = DateTime.Now.AddHours(1);
                timeSlot.routineScheduleMode = SelectedRoutine.routineScheduleIndex;


                SelectedRoutine.rountineTiming.Add(timeSlot);
                SelectedRoutine = SelectedRoutine;
                NotifyOfPropertyChange(() => SelectedRoutine);
            }
            
        }

        public void toggleRoutineActive()
        {
            Console.WriteLine("Trigger - "+ HoverOverRoutine.routineName);
            
            HoverOverRoutine.routineActive = !HoverOverRoutine.routineActive;
            refreshList();
            Console.WriteLine(HoverOverRoutine.routineActive);
        }


        #region Media Slot Logic

        public void AddMediaSlot()
        {
            RoutineModel.routineSlot slot = new RoutineModel.routineSlot();
            SelectedRoutine.routine.Add(slot);
            NotifyOfPropertyChange(() => SelectedRoutine.routine);
            NotifyOfPropertyChange(() => SelectedRoutine);
            NotifyOfPropertyChange(() => routines);
            refreshList();
            refreshMediaSlotList();
        }

        

        public void refreshMediaSlotList()
        {
            //routines.Clear();
            //NotifyOfPropertyChange(() => routines);
            mediaslots = new BindableCollection<RoutineModel.routineSlot>(SelectedRoutine.routine);
            NotifyOfPropertyChange(() => mediaslots);
        }

        public void RemoveItem(object i)
        {
            SelectedRoutine.routine.Remove(i as RoutineModel.routineSlot);
            NotifyOfPropertyChange(() => mediaslots);
            refreshList();
            refreshMediaSlotList();
         
        }

        #endregion




    }
}
