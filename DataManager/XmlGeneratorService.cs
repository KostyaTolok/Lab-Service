using System;
using System.ServiceProcess;
using ServiceConfigurationManager;
using System.Threading;
using System.IO;
using DataManager.DataManagerOptions;

namespace DataManager
{
    public partial class XmlGeneratorService : ServiceBase
    {
        private XmlGenerator generator;
        private OptionsManager optionsManager;
        private readonly Options options;

        public XmlGeneratorService()
        {
            InitializeComponent();
            //Сервис по генерации xml также использует ранее написанный OptionsManager
            optionsManager = new OptionsManager(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataManagerConfig.xml"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataManagerConfigSchema.xsd"));

            options = optionsManager.GetOptions<Options>();

            CanStop = options.ServiceOptions.CanStop;
            CanPauseAndContinue = options.ServiceOptions.CanPauseAndContinue;
            AutoLog = options.ServiceOptions.AutoLog;
        }

        protected override void OnStart(string[] args)
        {
            generator = new XmlGenerator(options);
            Thread serviceThread = new Thread(new ThreadStart(generator.Start));
            serviceThread.Start();
        }

        protected override void OnStop()
        {
            generator.Stop();
            Thread.Sleep(1000);
        }
    }
}
