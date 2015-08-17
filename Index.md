# Introduction #

Learning the Code


## Step By Step ##
  * Open your Visual Studio
  * Create a New Project (File->New->Project or CRTL+SHIFT+N)
  * Select "Windows Service Choice"
  * Name your project, in case is ExampleSerice
  * In your Solution Explorer must have these files
  1. Properties
  1. References
  1. Program.cs
  1. Service1.cs
  * First thing to do organize the "cs" files, so rename Service1.cs to Service.cs
  * Create a new Class called "Process.cs"
  * Now we need edit the Service.cs use the right buttom to show the menu and select View Code;
  * Add the follow code on the file
```c#

static void Main(string[] args)
{
}
```
  * Inside the "static void Main" put the follow code used to disable the threads on debug mode
```c#

#if DEBUG
#else
#endif
```
  * Open the Program.cs and copy all the content inside the main() e paste on inside of #else statement, should be like this
```c#

static void Main(string[] args)
{
#if DEBUG
#else
ServiceBase[] ServicesToRun;
ServicesToRun = new ServiceBase[]
{
new Service()
};
ServiceBase.Run(ServicesToRun);
#endif
}
```
  * now you must delete de Program.cs
  * so, you need create the variables for the service use;
```c#

public const string NAME = "ExampleService";
public const string DISPLAY_NAME = "Service Example";

static void Main(string[] args)
{
#if DEBUG
#else
//Verify if is a service call or a user callVerifica se a chamada do Serviço foi ou não chamado pelo usuário.
if (!Environment.UserInteractive) { //service call => run the service
ServiceBase[] ServicesToRun;
ServicesToRun = new ServiceBase[] { new Service() };
ServiceBase.Run(ServicesToRun);
}else { //user call => install and uninstall

}
#endif
}
```
  * Add another Windows Service (on project click right buttom and New->Add Item, or CRTL+SHIFT+A)
  * Call this Windows Service of "ProjectInstaller.cs"
  * Now go to the Code of project Instaler using the right buttom to show the menu and select View Code;
  * Replace all the content of ProjectInstaller for the code bellow
```c#

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

namespace ServiceWSFeixe
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
```
  * Dont forgot to resolve the Reference adding the follow reference "System.Configuration.Install"
  * Now lets complete the #else/else statemant with the cold below
```c#

else { //user call => install and uninstall
ServiceController sc = new ServiceController(NAME);
if (!ServiceExists())
{
if (DialogResult.OK == MessageBox.Show("Do you want install " + DISPLAY_NAME + "?", DISPLAY_NAME, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
{
try
{
Trace.WriteLine("Installing service \"" + DISPLAY_NAME + "\"...");
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
if (DialogResult.OK == MessageBox.Show("Do you want uninstall " + DISPLAY_NAME + "?", DISPLAY_NAME, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
{
try
{
Trace.WriteLine("Unistalling service \"" + DISPLAY_NAME + "\"...");
ProjectInstaller.Uninstall();
}
catch (Exception ex)
{
Trace.TraceError(ex.Message);
}
}
}
}
```
  * Now you need create the ServiceExists() on end of the Service.cs, the method looks like this
```c#

private static bool ServiceExists()
{
foreach (ServiceController sc in ServiceController.GetServices())
if (sc.ServiceName == NAME)
return true;
return false;
}
```
  * Now we need configure the App.config for use the Trace() to log or errors, and popule or Process.cs with the threads.
  * Click with right buttom on the project name Add->New Item select "Application Configuration File" use the name App.config, and replace the content by the follow code.
```xml

<?xml version="1.0" encoding="utf-8" ?>
<configuration>
<configSections>
<sectionGroup name="applicationSettings" type="System.Configuration.ApplicationSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" >
<section name="ExampleService.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />


Unknown end tag for &lt;/sectionGroup&gt;




Unknown end tag for &lt;/configSections&gt;



<system.diagnostics>
<trace autoflush="true">
<listeners>
<add type="System.Diagnostics.TextWriterTraceListener" name="TextWriter" initializeData="C:\log\trace.log">


Unknown end tag for &lt;/add&gt;




Unknown end tag for &lt;/listeners&gt;




Unknown end tag for &lt;/trace&gt;


</system.diagnostics>

<applicationSettings>
<ExampleService.Properties.Settings>
<setting name="INILogPath" serializeAs="String">
<value>C:\Log

Unknown end tag for &lt;/value&gt;




Unknown end tag for &lt;/setting&gt;


</ExampleService.Properties.Settings>


Unknown end tag for &lt;/applicationSettings&gt;




Unknown end tag for &lt;/configuration&gt;


```
  * Now we need create our thread class, go to the Process.cs and make a fully static class, and create the methods Start() Stop() like above
```c#

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExampleService
{
public static class Process
{
static Process() { }

public static void Start() { }

public static void Stop() { }
}
}
```
  * So, we need implement the methods like above.
```c#

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace ExampleService
{
public static class Process
{
static Process() { }

//Start the thread and call the service
public static void Start()
{
Trace.WriteLine("Processo Start()");
Thread thread = new Thread(new ThreadStart(doWork));
thread.Start();
Thread thread2 = new Thread(new ThreadStart(doWorks));
thread2.Start();
}

//stop the thread and stop the service
public static void Stop()
{
}

//action called by the thread and debugmode
public static void doWork()
{
for (int x = 0; x <= 10000; x++)
{
Trace.WriteLine("x = " + x);
}
}

//action called by the thread and debugmode
public static void doWorks()
{
for (int x = 10000; x > 1; x--)
{
Trace.WriteLine("Y = " + x);
}
}
}
}
```
  * So, we need complete our debug mode, will look like this.
```c#

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
if (DialogResult.OK == MessageBox.Show("Do you want install " + DISPLAY_NAME + "?", DISPLAY_NAME, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
{
try
{
Trace.WriteLine("Installing service \"" + DISPLAY_NAME + "\"...");
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
if (DialogResult.OK == MessageBox.Show("Do you want uninstall" + DISPLAY_NAME + "?", DISPLAY_NAME, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
{
try
{
Trace.WriteLine("Unistalling service \"" + DISPLAY_NAME + "\"...");
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
```