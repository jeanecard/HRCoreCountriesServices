using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.AzureAppServices;

namespace HRCoreCountriesWebAPI2
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 1- Create WebHost
        /// 2- Configure settings
        /// 3- Configure Logger
        /// 4- Run WebHost
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //1-
            IWebHostBuilder webHostBuilder = CreateWebHostBuilder(args);
            //2-
            webHostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                          optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
            });
            //3-
            //webHost.ConfigureLogging((hostingContext, logging) =>
            //{
            //    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            //    logging.AddConsole();
            //    logging.AddDebug();
            //});
            //4-
            IWebHost webHost = webHostBuilder.Build();
            webHost.Run();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>().ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddDebug();
                    logging.AddConsole();
                    logging.AddAzureWebAppDiagnostics();
                });
    }
}
