using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using FireflyGuardian.ServerResources;
using FireflyGuardian.ViewModels;
using Newtonsoft.Json;

namespace FireflyGuardian
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
            //base.OnStartup(sender, e);
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            //Shutsdown recieving thread for UDP server. Otherwise program will constantly run on thread.
            
            ServerManagement.stopAll();
            base.OnExit(sender, e);
        }
    }
}
