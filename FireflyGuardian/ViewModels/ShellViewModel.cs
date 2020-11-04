using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using FireflyGuardian.Models;

namespace FireflyGuardian.ViewModels
{

    public delegate void NotifyDestroyCurrentView();
    public delegate void NotifyCreatedCurrentView();
    class ShellViewModel : Conductor<Object>
    {

        /*public static IPLogModel ipLog = new IPLogModel();*/
       /* private static UDPConnectionModel _uDPConnection;
        public static UDPConnectionModel udpConnection { get { return _uDPConnection; } }
        public static SettingsModel settings;*/
        public int menuWidth { get; set; }
        public BindableCollection<PageModel> _pages = new BindableCollection<PageModel>();
        public static event NotifyDestroyCurrentView NotfiyDestoryView;
        public static event NotifyCreatedCurrentView NotfiyNewView;
        public static Type activePageType;
        DashboardViewModel DashboardPage = new DashboardViewModel();
        DeviceNetworkViewModel DeivceNetworkViewPage = new DeviceNetworkViewModel();
        MediaPoolViewModel MediaPoolPage = new MediaPoolViewModel();
        SettingsViewModel SettingsPage = new SettingsViewModel();
        public ShellViewModel()
        {
            generatePages();
            ActivateItem(Pages[0].View);
            StartUpView();

        }


        public void StartUpView()
        {
            ServerResources.DataAccess.Init init = new ServerResources.DataAccess.Init();
            ServerResources.ServerManagement.Init();
            if (init.firstTimeStartUp)
            {
                menuWidth = 0;
                NotifyOfPropertyChange(() => menuWidth);
                ActivateItem(new InitSetupWindowViewModel(init, this));
            }
            else
            {
                ExitSetupView();
            }
        }

        public void ExitSetupView()
        {
            ActivateItem(Pages[0].View);
            menuWidth = 40;
            NotifyOfPropertyChange(() => menuWidth);
        }



        public void generatePages()
        {
            Pages.Add(new PageModel { icon = "\uF404", name = "Dashboard", reloadOnActive = false, View = DashboardPage }); ;
            Pages.Add(new PageModel { icon = "\uF0B9", name = "Device Network", reloadOnActive = false, View = DeivceNetworkViewPage });
            Pages.Add(new PageModel { icon = "\uEC51", name = "File Management", reloadOnActive = true, View = MediaPoolPage });
            Pages.Add(new PageModel { icon = "\uE823", name = "Schedule", reloadOnActive = false });
            Pages.Add(new PageModel { icon = "\uE713", name = "Settings", reloadOnActive = false, View = SettingsPage }); //View = new SettingsViewModel()
        }

        public BindableCollection<PageModel> Pages
        {
            get { return _pages; }
            set { _pages = value; }
        }
        private PageModel _pageModel;

        public PageModel SelectedPage
        {
            get { return _pageModel; }
            set
            {
                
                _pageModel = value;
              
                NotifyOfPropertyChange(() => SelectedPage);
                NotifyOfPropertyChange(() => Pages);
                PageSwitch(_pageModel);
            }
        }



        #region PageNavigation
        /* When a page is switched on the page menu at the side of the screen, this function is triggered
         * It then triggers the PageSwitch method which loads the model view of the page into the active Item
         * to be displayed */
        
        public void PageSwitch(PageModel activePage)
        {
            NotfiyDestoryView.Invoke();
            //If page does not need a active reload, load the same page that was generated at the start of the program
            //else, get the type of the page, and generate a new instance of the page
            if (!activePage.reloadOnActive)
            {
                
                ActivateItem(activePage.View);
            }
            else
            {
                Console.WriteLine("2");
                Type t = activePage.View.GetType();
                activePage.View = Activator.CreateInstance(t);
                ActivateItem(activePage.View);
            }
            activePageType = activePage.View.GetType();
            NotfiyNewView.Invoke();





        }

        #endregion


    }
}
