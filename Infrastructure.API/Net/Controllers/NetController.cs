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
using Infrastructure.Application.Net.UseCases.RunTcpServer;

namespace Infrastructure.API.Net.Controllers
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
                RunTcpServerCommand runTcpServerCommand =  message.DeserializeParamValues<RunTcpServerCommand>();
                return this.netApplication.RunTcpServer(message.GetClient(), runTcpServerCommand);
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
