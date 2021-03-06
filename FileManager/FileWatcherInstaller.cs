﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Linq;
using System.Threading.Tasks;

namespace FileManager
{
    [RunInstaller(true)]
    public partial class FileWatcherInstaller : Installer
    {
        ServiceInstaller installer;
        ServiceProcessInstaller processInstaller;
        public FileWatcherInstaller()
        {
            InitializeComponent();
            installer = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;
            installer.StartType = ServiceStartMode.Manual;
            installer.ServiceName = "FileManager";
            Installers.Add(installer);
            Installers.Add(processInstaller);
        }
    }
}
