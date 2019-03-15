using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using ReportingService.Adapter;
using ReportingService.Configuration;

namespace ReportingService
{
    public partial class ReportGeneratorService : ServiceBase
    {

        private readonly Timer _timer;
        private readonly Logger _logger;
        private readonly ReportGenerator _reportGenerator;
        private readonly string _path;
        private readonly int _interval;

        public ReportGeneratorService()
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();
            _logger.Info("[ReportGeneratorService] Service started");

            if (ConfigurationManager.GetSection("reportGeneratorSection") is ReportGeneratorSection config)
            {
                _path = config.Directory.Path;
                _interval = config.Interval.Interval;
                _logger.Info($"[ReportGeneratorService] Path = '{_path}'  interval='{_interval}' was read from config file");
                _reportGenerator = new ReportGenerator(new Writer(_path), new TradeWorker(new TradeAdapter(), new PeriodWorker()), _logger);
                _logger.Info($"[ReportGeneratorService] ReportGenerator was created with path = '{_path}'");
                _timer = new Timer(WorkProcedure);
                InitializeComponent();
            }
            else
            {
                _logger.Error("[ReportGeneratorService] Can't read config file");
                throw new ConfigurationErrorsException();
            }
        }

        protected override void OnStart(string[] args)
        {
            var startTime = 0;
           _timer.Change(startTime, _interval * 60000);
        }

        private void WorkProcedure(object target)
        {
            _reportGenerator.GenerateReport();
        }

        protected override void OnStop()
        {
            _timer.Change(Timeout.Infinite, 0);
        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }
    }
}
