using System;

namespace Infrastructure.Application.Net.UseCases.RunTcpServer
{
    public class RunTcpServerCommand
    {

        /// <summary>
        /// Puerto en donde escuchara el servidor tcp que se crea
        /// </summary>
        /// <value></value>
        public int Port { get; set; }


        public int ForwardPort { get; set; }

        public ForwardMessageType MessageType { get; set; }

    }
}