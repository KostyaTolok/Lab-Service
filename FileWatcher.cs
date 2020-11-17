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

namespace Lab2_service
{
    public partial class FileWatcher : ServiceBase
    {
        private Service service;
        private readonly OptionsManager optionsManager;
        private readonly Options options;
        public FileWatcher()
        {
            InitializeComponent();
            optionsManager = new OptionsManager(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json")); //Создаем объект OptionsManager и передаем ему                                                                                                  
                                                                                                                    //путь на файл конфигурации
            options = new Options       //Создаем объект Options и присваиваем его полям модели настроек через метод GetOptions                                                                               
            {
                ArchiveOptions = optionsManager.GetOptions<ArchiveOptions>(),
                PathOptions = optionsManager.GetOptions<PathOptions>(),
                ServiceOptions = optionsManager.GetOptions<ServiceOptions>()
            };
            CanStop = options.ServiceOptions.CanStop;                           //Используем настройки
            CanPauseAndContinue = options.ServiceOptions.CanPauseAndContinue;
            AutoLog = options.ServiceOptions.AutoLog;
        }

        protected override void OnStart(string[] args)
        {
            service = new Service(options);
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
