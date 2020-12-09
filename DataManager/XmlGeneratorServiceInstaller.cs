using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace DataManager
{
    [RunInstaller(true)]
    public partial class XmlGeneratorServiceInstaller : Installer
    {
        ServiceInstaller installer;
        ServiceProcessInstaller processInstaller;
        public XmlGeneratorServiceInstaller()
        {
            InitializeComponent();
            installer = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;
            installer.StartType = ServiceStartMode.Manual;
            installer.ServiceName = "DataManager";
            Installers.Add(installer);
            Installers.Add(processInstaller);
        }
    }
}
