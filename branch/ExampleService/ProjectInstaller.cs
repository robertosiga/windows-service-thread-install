using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Configuration.Install;

namespace ExampleService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            ServiceProcessInstaller spi = new ServiceProcessInstaller();
            spi.Account = ServiceAccount.LocalSystem;
            System.ServiceProcess.ServiceInstaller si = new System.ServiceProcess.ServiceInstaller();
            si.ServiceName = Service.NAME;
            si.DisplayName = Service.DISPLAY_NAME;
            si.Description = Service.DISPLAY_NAME;
            si.StartType = ServiceStartMode.Automatic;
            Installers.Add(spi);
            Installers.Add(si);
        }
        public static void Install()
        {
            string[] s = { Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + Service.NAME + ".exe" };
            ManagedInstallerClass.InstallHelper(s);
            ServiceController sc = new ServiceController(Service.NAME);
            sc.Start();
        }

        public static void Uninstall()
        {
            string[] s = { "/u", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + Service.NAME + ".exe" };
            ManagedInstallerClass.InstallHelper(s);
        }
    }
}

