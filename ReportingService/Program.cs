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
                ReportGeneratorService service1 = new ReportGeneratorService();
                service1.TestStartupAndStop(args);
            }
            else
            {
                ServiceBase.Run(new ReportGeneratorService());
            }

            
        }
    }
}
