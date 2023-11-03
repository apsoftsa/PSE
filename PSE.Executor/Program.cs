using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PSE.Executor
{

    internal static class Program
    {

        [STAThread]
        static void Main()
        {            
            var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((hostContext, config) =>
            {
                config.AddJsonFile("AppSettings.json");
            })
            .ConfigureServices((hostContext, services) => { })
            .Build();            
            ApplicationConfiguration.Initialize();
            Application.Run(new FormMain(host.Services.GetRequiredService<IConfiguration>()));
        }

    }

}