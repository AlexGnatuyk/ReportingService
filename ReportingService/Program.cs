using System;
using System.ServiceProcess;

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
