using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using ApplicationLib.DataTransferObjects;
using ApplicationLib.Microservices;
using ApplicationLib.Microservices.Controllers;
using ApplicationLib.Microservices.Messages;
using Infrastructure.Application.Net;

namespace Infrastructure.API.EmailService.Controllers
{
    [MicroserviceController("Net")]
    [Route("api/[controller]")]
    public class NetController: IMicroserviceController
    {
        private INetApplication netApplication;

        public NetController(INetApplication netApplication)
        {
            this.netApplication = netApplication;
        }


        public async Task<object> RunTcpServer(MicroserviceMessage message)
        {
            try
            {
                this.netApplication.RunTcpServer(message.GetClient(), 6000, "localhost", 8000, ForwardMessageType.ByteArray); 
                return "Hola";
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
