using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ResourceController
{
    [RunInstaller(true)]
    class GlobalInstaller : Installer
    {
        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller serviceProcessInstaller;
        public GlobalInstaller()
        {
            //TODO: service should be started not from LocalSystem but from custom user created during installation
            serviceProcessInstaller = new ServiceProcessInstaller { Account = ServiceAccount.LocalService };
            serviceInstaller = new ServiceInstaller { ServiceName = "SharedHardware.Controller",
                StartType = ServiceStartMode.Automatic,
                Description = "Service sends keepAlive to SharedHardware server and receive computation for execution in restricted context"                
            };
            Installers.Add(serviceInstaller);
            Installers.Add(serviceProcessInstaller);
        }

    }
}
