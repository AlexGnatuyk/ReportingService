using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace ReportingService
{
    [RunInstaller(true)]
    public partial class ReportGeneratorServiceInstaller : Installer
    {
        public ReportGeneratorServiceInstaller()
        {
            InitializeComponent();
            var serviceInstaller = new ServiceInstaller();
            serviceInstaller.ServiceName = " ReportGeneratorService";
            serviceInstaller.DisplayName = " Report Generator Service";
            serviceInstaller.DelayedAutoStart = true;
            serviceInstaller.StartType = ServiceStartMode.Manual;

            this.Installers.Add(serviceInstaller);

            var serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalService;

            this.Installers.Add(serviceProcessInstaller);
        }
    }
}
