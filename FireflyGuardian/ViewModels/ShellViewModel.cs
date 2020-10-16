using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using FireflyGuardian.Models;

namespace FireflyGuardian.ViewModels
{
    class ShellViewModel : Conductor<Object>
    {

        /*public static IPLogModel ipLog = new IPLogModel();*/
       /* private static UDPConnectionModel _uDPConnection;
        public static UDPConnectionModel udpConnection { get { return _uDPConnection; } }
        public static SettingsModel settings;*/
        public int menuWidth { get; set; }
        public BindableCollection<PageModel> _pages = new BindableCollection<PageModel>();

        public ShellViewModel()
        {
           /* menuWidth = 250;
           *//* settings = new SettingsModel();*//*
            NotifyOfPropertyChange(() => menuWidth);*/
            generatePages();
            ActivateItem(Pages[0].View);
            /*_uDPConnection = new UDPConnectionModel();*/
            //StartUpView();
        }

        /*public void StartUpView()
        {
            DataAccess.Init init = new DataAccess.Init();
            if (init.firstTimeStartUp)
            {
                menuWidth = 0;
                NotifyOfPropertyChange(() => menuWidth);
                ActivateItem(new InitSetupWindowViewModel(init, this));
            }
        }*/

        /*public void ExitSetupView()
        {
            ActivateItem(Pages[0].View);
            menuWidth = 250;
            NotifyOfPropertyChange(() => menuWidth);
        }*/


        public void generatePages()
        {
            Pages.Add(new PageModel { icon = "\uF404", name = "Dashboard", reloadOnActive = false, View = new DashboardViewModel()  }); ;
            Pages.Add(new PageModel { icon = "\uF0B9", name = "Device Network", reloadOnActive = false, View = new DeviceNetworkViewModel() });
            Pages.Add(new PageModel { icon = "\uEC51", name = "File Management", reloadOnActive = false });
            Pages.Add(new PageModel { icon = "\uE823", name = "Schedule", reloadOnActive = false });
            Pages.Add(new PageModel { icon = "\uE713", name = "Settings", reloadOnActive = true }); //View = new SettingsViewModel()
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
            //If page does not need a active reload, load the same page that was generated at the start of the program
            //else, get the type of the page, and generate a new instance of the page
            if (!activePage.reloadOnActive)
            {
                ActivateItem(activePage.View);
            }
            else
            {

                Type t = activePage.View.GetType();
                activePage.View = Activator.CreateInstance(t);
                ActivateItem(activePage.View);
            }


        }

        #endregion


    }
}
