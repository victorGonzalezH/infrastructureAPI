using Utils.Net.Tcp;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ApplicationLib.Microservices;
using System.Text;
using System.Text.Json;


namespace Infrastructure.Application.Net
{

    public class NetApplication: INetApplication
    {

        ///
        /// Listado de servidores tcp
        ///
        private List<ForwardTransport> forwardTransports;


        public NetApplication()
        {
            this.forwardTransports = new List<ForwardTransport>();
        }

        
        public void RunTcpServer(IMicroserviceClient client, int port, string forwardHost, int forwardPort = 0, ForwardMessageType messageType = ForwardMessageType.String)
        {
             TcpServer tcpServer;
             Thread t = new Thread(delegate ()
             {
                tcpServer = new TcpServer(port);
                tcpServer.Id = this.forwardTransports.Count + 1;
                tcpServer.TcpMessageArrived +=  OnTcpMessageReceived;
                tcpServer.TcpClientDisconnected += OnTcpClientDisconnected;

                if(messageType == ForwardMessageType.ByteArray)
                {
                    tcpServer.InputMessagesType = ServerMessagesTypes.ByteArray;
                    tcpServer.OutputMessagesType = ServerMessagesTypes.ByteArray;
                }


                // Si el puerto para redireccionamiento es mayor que cero, entonces eso significa el microservicio
                // que solicito crear el servidor tcp estara escuchando los mensajes retransmitidos en ese puerto
                // para ello, se crea un cliente tcp para la retransmision de los mensajes
                if (forwardPort > 0 && forwardHost != null)
                {
                   this.forwardTransports.Add(new ForwardTransport(tcpServer, client, forwardHost, forwardPort, messageType));
                }


                tcpServer.StartListener();
                
             });
            
            t.Start();
        }



        private async Task OnTcpClientDisconnected(object source, TcpClientDisconnectedEventArgs tcpEvent)
        {
            TcpServer tcpServer = source as TcpServer;
            if(tcpServer != null)
            {
                ForwardTransport ft = this.forwardTransports.FirstOrDefault( f => f.TcpServer.Id == tcpServer.Id);
                if (ft != null)
                {
                    
                }
            }
        }


        private async Task OnTcpMessageReceived(object source, TcpMessageArrivedEventArgs tcpEvent)
        {
            TcpServer tcpServer = source as TcpServer;
            if(tcpServer != null)
            {
                ForwardTransport ft = this.forwardTransports.FirstOrDefault( f => f.TcpServer.Id == tcpServer.Id);
                if (ft != null)
                {
                    if(ft.TcpClient == null)
                    {
                        ft.BuildTcpClient();
                    }

                    // Si el tipo de mensaje para reenviar es de tipo cadena
                    if(ft.MessageType == ForwardMessageType.String)
                    {
                        string message = tcpEvent.RawString;
                        // Si el tipo de microservicio cliente es nestjs, entonces el mensaje se pasa a el formato que acepta
                        // nestjs
                        if( ft.Client.GetClientType() == MicroserviceClientType.Nestjs)
                        {
                            message = BuildNestjsMessage(message);
                        }

                        ft.TcpClient.SendMessage(message);
                    }
                    else if(ft.MessageType == ForwardMessageType.ByteArray)
                    {
                        byte[] rawData = tcpEvent.RawData;
                        // Si el tipo de microservicio cliente es nestjs, entonces el mensaje se pasa a el formato que acepta
                        // nestjs
                        if( ft.Client.GetClientType() == MicroserviceClientType.Nestjs)
                        {
                            string encodedMessage = Encoding.ASCII.GetString(rawData, 0, rawData.Length);
                            encodedMessage = BuildNestjsMessage(encodedMessage);
                            ft.TcpClient.SendMessage(encodedMessage);
                        }
                        
                        ft.TcpClient.SendMessage(rawData);
                    }
                }
            }
        }



        private string BuildNestjsMessage(string message)
        {
            NestjsRequest request = new NestjsRequest() { data = message, pattern = "Test" };
            string stringRequest = JsonSerializer.Serialize<NestjsRequest>(request);
            int length = stringRequest.Length;
            stringRequest = length.ToString() + "#" + stringRequest;
            return stringRequest;
        }

    }

}