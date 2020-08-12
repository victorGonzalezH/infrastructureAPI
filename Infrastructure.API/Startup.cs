using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Infrastructure.API.Files;
using Infrastructure.Application.Email;
using Infrastructure.Application.Files.Export;
using Infrastructure.Application.Images;
using Infrastructure.Application.Templates;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence;

using Infrastructure.Domain;
using Utils.Http;
using Utils.IO.Templates;
using Infrastructure.Application.Files;
using Utils.IO.Files;
using Utils.IO.Files.Exports;
using Utils.Email;
using Utils.IO.Templates.Html;
using Utils.IO.Images;
using Utils.DB;



namespace Infrastructure.API
{
    public class Startup
    {

         #region Propiedades

        public IConfiguration Configuration { get; }

        #endregion

        /// <summary>
        /// Configuracion de la aplicacion
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
                

                services.AddControllers();


                //Se agrega la configuracion del servicio de archivos
                services.Configure<FileApiSettings>(Configuration.GetSection("FileApiSettings"));

                //Se agrega la configuracion de correo electronico para el servicio de correo
                services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

                services.AddSingleton<EmailSettings>(sp => sp.GetRequiredService<IOptions<EmailSettings>>().Value);

                //Se agrega el servicio de aplicacion para la gestion de archivos 
                services.AddTransient(typeof(IFilesApplication), typeof(FilesApplication));


                //Se agrega el servicio de manejador de archivos
                services.AddTransient(typeof(IFilesManager), typeof(FilesManager));

                
                services.AddTransient(typeof(IInfrastructureUnitOfWork), typeof(UnitOfWork));

                services.AddTransient(typeof(IInfrastructureContext), typeof(InfrastructureContext));

                services.Configure<List<ConnectionString>>(Configuration.GetSection("ConnectionStrings"));
                
                services.AddSingleton<List<ConnectionString>>(sp => sp.GetRequiredService<IOptions<List<ConnectionString>>>().Value);

                services.AddSingleton<IInfrastructureConnectionString>(sp => 
                { 
                    var connectionStrings = sp.GetRequiredService<IOptions<List<ConnectionString>>>().Value;
                    ConnectionString connectionString = connectionStrings.FirstOrDefault( cs => cs.Name == "infrastructure");
                    return new InfrastructureConnectionString(connectionString){  };
                });


                //Se agrega el servicios de repositorio de los archivos (clase File)
                services.AddTransient(typeof(SeedWork.IRepository<Infrastructure.Domain.Files.File>), typeof(Infrastructure.Persistence.Repositories.Repository<Infrastructure.Domain.Files.File>));



                //Se agrega el servicio del cliente de correo
                services.AddTransient(typeof(IEmailClient), typeof(EmailClient));


                //Se agrega el servicio del cliente de SMTP
                services.AddTransient(typeof(ISmtpClientWrapper), typeof(SmtpClientWrapper));

                   //Se agrega el servicio de la aplicacion de correo
                services.AddTransient(typeof(IEmailApplication), typeof(EmailApplication));


                //Se agrega el servicio de Analizador de HTMl
                services.AddTransient(typeof(IHtmlParser), typeof(HtmlParser));

                 //Se agrega el manejador de las plantillas de correo
                services.AddTransient<ITemplatesManager, TemplatesManager>();
                    
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
