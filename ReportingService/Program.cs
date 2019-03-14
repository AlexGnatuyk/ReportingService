using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            if (Environment.UserInteractive)
            {
                ReportGeneratorService reportGeneratorService = new ReportGeneratorService();
                reportGeneratorService.TestStartupAndStop(args);
            }
            else
            {
                ServiceBase.Run(new ReportGeneratorService());
            }

            
        }
    }
}
