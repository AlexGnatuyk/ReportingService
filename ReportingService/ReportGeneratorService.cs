using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
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
                _logger.Info($"[ReportGeneratorService] From config file was readed path = '{_path}' interval='{_interval}'");
                _reportGenerator = new ReportGenerator(_path, _logger);
                _timer = new Timer(WorkProcedure);
                InitializeComponent();
            }
            else
            {
                _logger.Error("[ReportGeneratorService] Can not read config file");
            }
        }

        protected override void OnStart(string[] args)
        {
           _timer.Change(0, _interval * 1000);
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
