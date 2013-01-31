using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;

namespace ExampleService
{
    public partial class Service : ServiceBase
    {
        public const string NAME = "ExampleService";
		public const string DISPLAY_NAME = "Service Example";

        public Service()
        {
            InitializeComponent();
        }       

		static void Main(string[] args)
		{
		#if DEBUG
            Process.doWork();
            Process.doWorks();
		#else
			//Verify if is a service call or a user callVerifica se a chamada do Serviço foi ou não chamado pelo usuário.
			if (!Environment.UserInteractive) { //service call => run the service
				ServiceBase[] ServicesToRun;
				ServicesToRun = new ServiceBase[] { new Service() };
				ServiceBase.Run(ServicesToRun);
		   }else { //user call => install and uninstall 
               ServiceController sc = new ServiceController(NAME);
               if (!ServiceExists())
               {
                   if (DialogResult.OK == MessageBox.Show("Deseja instalar o serviço " + DISPLAY_NAME + "?", DISPLAY_NAME, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                   {
                       try
                       {
                           Trace.WriteLine("Instalando o serviço \"" + DISPLAY_NAME + "\"...");
                           ProjectInstaller.Install();
                       }
                       catch (Exception ex)
                       {
                           Trace.TraceError(ex.Message);
                       }
                   }
               }
               else
               {
                   if (DialogResult.OK == MessageBox.Show("Deseja desinstalar o serviço " + DISPLAY_NAME + "?", DISPLAY_NAME, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                   {
                       try
                       {
                           Trace.WriteLine("Desinstalando o serviço \"" + DISPLAY_NAME + "\"...");
                           ProjectInstaller.Uninstall();
                       }
                       catch (Exception ex)
                       {
                           Trace.TraceError(ex.Message);
                       }
                   }
               }
		   }
		#endif
		}

        protected override void OnStart(string[] args)
        {
            Process.Start();
        }

        protected override void OnStop()
        {
            Process.Stop();
        }

        private static bool ServiceExists()
        {
            foreach (ServiceController sc in ServiceController.GetServices())
                if (sc.ServiceName == NAME)
                    return true;
            return false;
        }
    }
}
