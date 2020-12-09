using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ApplicationLib.Microservices;

namespace Infrastructure.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            // Creacion del host para los microservicios
            MicroserviceHost.Build<StartupCommunication>("appsettings.json").UseTcp<TcpSettings>().Run();
            
            // Creacion del servidor Web API
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
