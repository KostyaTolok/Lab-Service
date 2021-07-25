using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ServiceConfigurationManager;

namespace FileManager
{
    public partial class FileWatcher : ServiceBase
    {
        private FileTransferService service;
        private readonly OptionsManager optionsManager;
        private readonly Options options;
        public FileWatcher()
        {
            InitializeComponent();
            optionsManager = new OptionsManager(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileManagerConfig.xml"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileManagerConfigSchema.xsd")); //Создаем объект OptionsManager и передаем ему                                                                                                  
                                                                                                     //путь на файл конфигурации и валидации
            options = optionsManager.GetOptions<Options>(); //Создаем объект Options и присваиваем его полям модели настроек через метод GetOptions
            CanStop = options.ServiceOptions.CanStop;                           //Используем настройки
            CanPauseAndContinue = options.ServiceOptions.CanPauseAndContinue;
            AutoLog = options.ServiceOptions.AutoLog;
        }

        protected override void OnStart(string[] args)
        {
            service = new FileTransferService(options);
            Thread serviceThread = new Thread(new ThreadStart(service.Start));
            serviceThread.Start();
        }

        protected override void OnStop()
        {
            service.Stop();
            Thread.Sleep(1000);
        }
    }
}
