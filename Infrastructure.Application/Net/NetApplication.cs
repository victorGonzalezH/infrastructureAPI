using Utils.Net.Tcp;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ApplicationLib.Microservices;
using System.Text;
using System.Text.Json;
using System;
using Infrastructure.Application.Net.UseCases.RunTcpServer;

namespace Infrastructure.Application.Net
{
    /// <summary>
    /// Clase aplicacion para crear servidores tcp
    /// </summary>
    public class NetApplication: INetApplication
    {

        ///
        /// Listado de servidores tcp
        ///
        private List<ForwardTransport> forwardTransports;

        /// <summary>
        /// Lista de puertos usados
        /// </summary>
        private List<int> usedPorts;

        /// <summary>
        /// Crea un objeto Aplicacion NET
        /// </summary>
        public NetApplication()
        {
            this.forwardTransports = new List<ForwardTransport>();
            this.usedPorts = new List<int>();
        }

        /// <summary>
        /// Crea y ejecuta un servidor TCP
        /// </summary>
        /// <param name="client"></param>
        /// <param name="runTcpServerCommand"></param>
        /// <returns></returns>
        public bool RunTcpServer(IMicroserviceClient client, RunTcpServerCommand runTcpServerCommand)
        {
            return RunTcpServer(client, runTcpServerCommand.Port, "localhost", runTcpServerCommand.ForwardPort, runTcpServerCommand.MessageType);
        }

        /// <summary>
        /// Crea y ejecuta un servidor TCP
        /// </summary>
        /// <param name="client">Cliente microservicio</param>
        /// <param name="port">Puerto en donde estara escuchando el servidor TCP</param>
        /// <param name="forwardHost"></param>
        /// <param name="forwardPort"></param>
        /// <param name="messageType">Tipos de mensajes que estara escuchando y procesando</param>
        /// <returns></returns>        
        public bool RunTcpServer(IMicroserviceClient client, int port, string forwardHost, int forwardPort = 0, ForwardMessageType messageType = ForwardMessageType.String)
        {

            // Si el puerto no se esta usando
            if (!this.usedPorts.Exists( localPort => localPort == port ))
            {
                TcpServer tcpServer;
                Thread t = new Thread(delegate ()
                {
                    // Crea el servidor tcp
                    tcpServer = new TcpServer(port);
                    // identificador del servidor
                    tcpServer.Id = this.forwardTransports.Count + 1;
                    // Se agrega el evento al servidor de cuando se recibe un mensaje de un cliente tcp
                    tcpServer.TcpMessageArrived +=  OnTcpMessageReceived;
                    // Se agrega el evento al servidor de cuando se desconecta un cliente tcp
                    tcpServer.TcpClientDisconnected += OnTcpClientDisconnected;

                    if(messageType == ForwardMessageType.ByteArray)
                    {
                        tcpServer.InputMessagesType = ServerMessagesTypes.ByteArray;
                        tcpServer.OutputMessagesType = ServerMessagesTypes.ByteArray;
                    }


                    // Si el puerto para redireccionamiento es mayor que cero, entonces eso significa que el microservicio
                    // que solicito crear el servidor tcp estara escuchando los mensajes retransmitidos en ese puerto
                    // para ello, se crea un cliente tcp para la retransmision de los mensajes
                    if (forwardPort > 0 && forwardHost != null)
                    {
                        this.forwardTransports.Add(new ForwardTransport(tcpServer, client, forwardHost, forwardPort, messageType));
                    }


                    tcpServer.StartListener();
                    
                });
                
                t.Start();

                return true;
            }
            

            return false;
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
                // Se verifica si este mensaje se tiene que retransmitir, esto se verifica si
                // existe un forwardTransport con el servidor origen
                ForwardTransport ft = this.forwardTransports.FirstOrDefault( f => f.TcpServer.Id == tcpServer.Id);
                if (ft != null)
                {
                    // Si el cliente tcp para retransmitir no esta creado, se crea, esto se hace
                    // solo una vez
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
                            // De alguna manera se debe de especificar que los mensajes
                            // los restransmita en base64
                            string encodedMessage = Convert.ToBase64String(rawData);
                            encodedMessage = BuildNestjsMessage(encodedMessage);
                            ft.TcpClient.SendMessage(encodedMessage);
                        }
                        else
                        {
                            ft.TcpClient.SendMessage(rawData);
                        }
                        
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