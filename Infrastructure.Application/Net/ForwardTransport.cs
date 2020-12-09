using Utils.Net.Tcp;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using ApplicationLib.Microservices;

namespace Infrastructure.Application.Net
{

    public enum ForwardMessageType 
    {
        Char = 0,
        
        String = 1,
        
        Int = 2,

        Float = 3,

        Byte = 4,

        IntArray = 5,

        FloatArray = 6,

        ByteArray = 7
    }


    public class ForwardTransport 
    {
        private TcpServer tcpServer;

        private UtilTcpClient tcpClient;

        public TcpServer TcpServer
        {
            get {  return this.tcpServer; }
        }

        public UtilTcpClient TcpClient
        {
            get { return this.tcpClient; }
        }

        private IMicroserviceClient client;


        public IMicroserviceClient Client
        {
            get { return this.client; }
        }

        private string host;

        private int port;

        private ForwardMessageType messageType;

        
        public ForwardMessageType MessageType
        {
            get { return this.messageType; }
        }


        public ForwardTransport(TcpServer tcpServer, IMicroserviceClient client, string host, int port, ForwardMessageType messageType)
        {
            this.tcpServer  = tcpServer;
            this.client     = client;
            this.host       = host;
            this.port       = port;
            this.messageType = messageType;
        }


        public void BuildTcpClient()
        {
            this.tcpClient = new UtilTcpClient(this.host, this.port);
        }
    }

}