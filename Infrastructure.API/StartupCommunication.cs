using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System;

using Infrastructure.API.EmailService.Controllers;
using Infrastructure.API.Net.Controllers;
using Infrastructure.API.SMS.Controllers;
using Infrastructure.Application.Email;
using Infrastructure.Application.Net;
using Infrastructure.Application.SMS;
using Infrastructure.Application.Net.UseCases.RunTcpServer;

using Utils.Email;
using Utils.IO.Templates;
using Utils.IO.Templates.Html;
using ApplicationLib.Microservices;
using ApplicationLib.Microservices.Messages;
using ApplicationLib.Microservices.Controllers;

using Amazon.SimpleNotificationService;

namespace Infrastructure.API
{
    ///<summary>
    ///Clase que configura
    ///</summary>
    public class StartupCommunication : StartupCommunicationBase
    {

        public StartupCommunication()
        {

        }

        protected override StartupCommunicationBase ConfigureServices()
        {
            this.AddTransient<IEmailApplication, EmailApplication>()
            .AddTransient<IMicroserviceController, EmailsController>()
            .AddTransient<IMicroserviceController, NetController>()
            .AddTransient<IMicroserviceController, SmsController>()
            .AddTransient<IEmailClient, EmailClient>()
            .AddTransient<IHtmlParser, HtmlParser>()
            .AddTransient<ITemplatesManager, TemplatesManager>()
            .AddTransient<ISmtpClientWrapper, SmtpClientWrapper>()
            .AddTransient<ISmsApplication, SmsApplication>()
            .AddSingleton<INetApplication, NetApplication>()
            .AddTransient<IAmazonSimpleNotificationService>(s => new AmazonSimpleNotificationServiceClient(region: Amazon.RegionEndpoint.USEast2))
            .AddSingleton<EmailSettings>(sp => sp.GetRequiredService<IOptions<EmailSettings>>().Value)
            .Configure<EmailSettings>(Configuration.GetSection("EmailSettings"))
            .Configure<TcpSettings>(Configuration.GetSection("TcpSettings"))
            .Configure<TcpServerConfig>(Configuration.GetSection("TcpServerConfig"));

            return this;
        }


        protected async override Task OnMessageReceived(object source, MessageArrivedEventArgs messageEvent)
        {
            await base.OnMessageReceived(source, messageEvent);

        }

        protected override void OnMethodInvoked(string jsonResultFormat, IMicroserviceClient client)
        {
            client.Send(jsonResultFormat);
        }

    }
}
