using Utils.Net.Tcp;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
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
    public class NetApplication : INetApplication
    {

        ///
        /// Listado de servidores tcp
        ///
        private List<ForwardTransport> forwardTransports;

        /// <summary>
        /// Lista de puertos usados
        /// </summary>
        private List<int> usedPorts;


        private List<TcpServerExecManager> tcpServerExecManager;

        /// <summary>
        /// Tcp server configuration
        /// </summary>
        private TcpServerConfig tcpServerConfig;


        /// <summary>
        /// Crea un objeto Aplicacion NET
        /// </summary>
        public NetApplication(IOptionsSnapshot<TcpServerConfig> tcpServerConfigSnapshot)
        {
            this.forwardTransports = new List<ForwardTransport>();
            this.usedPorts = new List<int>();
            this.tcpServerExecManager = new List<TcpServerExecManager>();
            this.tcpServerConfig = tcpServerConfigSnapshot.Value;
        }

        /// <summary>
        /// Crea y ejecuta un servidor TCP
        /// </summary>
        /// <param name="client"></param>
        /// <param name="runTcpServerCommand"></param>
        /// <returns></returns>
        public int RunTcpServer(IMicroserviceClient client, RunTcpServerCommand runTcpServerCommand)
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
        public int RunTcpServer(IMicroserviceClient client, int port, string forwardHost, int forwardPort = 0, ForwardMessageType messageType = ForwardMessageType.String)
        {
            // Este microserviceCliente es el que solicito la creacion del servidor tcp. Se le anade la funcion
            // OnMicroserviceClientDisconnect para notificar a el servidor tcp que ya no hay ningun client microservicio
            // que estara escuchando los mensajes de los clientes tcp
            (client as MicroserviceClient).Disconnected += OnMicroserviceClientDisconnect;
            // Si el puerto no se esta usando
            if (!this.usedPorts.Exists(localPort => localPort == port))
            {
                TcpServer tcpServer = null;
                Thread t = new Thread(delegate ()
                {
                    // Crea el servidor tcp
                    tcpServer = new TcpServer(this.tcpServerConfig.HostNameOrIp, port);
                    // identificador del servidor
                    tcpServer.Id = this.forwardTransports.Count + 1;
                    // Se agrega el evento al servidor de cuando se recibe un mensaje de un cliente tcp
                    tcpServer.TcpMessageArrived += OnTcpMessageReceived;
                    // Se agrega el evento al servidor de cuando se desconecta un cliente tcp
                    tcpServer.TcpClientDisconnected += OnTcpClientDisconnected;
                    // Se agrega el evento al servidor de cuando se conecta un cliente tcp
                    tcpServer.TcpClientConnected += OnTcpClientConnected;
                    if (messageType == ForwardMessageType.ByteArray)
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

                    this.usedPorts.Add(port);
                    TcpServerExecManager tcpServerExecManager = this.tcpServerExecManager.FirstOrDefault(tcpServerExecManagerLocal => tcpServerExecManagerLocal.MicroserviceId == client.GetId());
                    tcpServerExecManager.SetTcpServer(tcpServer);
                    try
                    {
                        tcpServer.StartListener();
                    }
                    catch (ThreadAbortException ex)
                    {
                        // Clean-up code can go here.  
                        // If there is no Finally clause, ThreadAbortException is  
                        // re-thrown by the system at the end of the Catch clause.
                        Thread.ResetAbort();
                    }
                    catch (Exception ex)
                    {

                    }
                });

                t.Start();
                this.tcpServerExecManager.Add(new TcpServerExecManager(client.GetId(), t, port));
                return 0;
            }
            else
            {
                if (forwardPort > 0 && forwardHost != null)
                {
                    ForwardTransport forwardTransport = this.forwardTransports.FirstOrDefault(ft => ft.TcpServer.Port == port);
                    forwardTransport.TcpClient.Reconnect();
                }

                // El puerto ya se esta usando, se devuelve -1;
                return -1;
            }

        }


        private async Task OnTcpClientConnected(object source, TcpClientConnectedEventArgs tcpEvent)
        {
            MicroserviceClient client = new MicroserviceClient(tcpEvent.TcpResponder);

        }


        /// <summary>
        /// Evento que se ejecuta cuando se desconecta un cliente tcp
        /// </summary>
        /// <param name="source"></param>
        /// <param name="tcpEvent"></param>
        /// <returns></returns>
        private async Task OnTcpClientDisconnected(object source, TcpClientDisconnectedEventArgs tcpEvent)
        {
            TcpServer tcpServer = source as TcpServer;

            if (tcpServer != null)
            {
                ForwardTransport ft = this.forwardTransports.FirstOrDefault(f => f.TcpServer.Id == tcpServer.Id);
                if (ft != null)
                {

                }
            }
        }


        private async Task OnTcpMessageReceived(object source, TcpMessageArrivedEventArgs tcpEvent)
        {
            TcpServer tcpServer = source as TcpServer;
            if (tcpServer != null)
            {
                // Se verifica si este mensaje se tiene que retransmitir, esto se verifica si
                // existe un forwardTransport con el servidor origen
                ForwardTransport ft = this.forwardTransports.FirstOrDefault(f => f.TcpServer.Id == tcpServer.Id);
                if (ft != null)
                {
                    // Si el cliente tcp para retransmitir no esta creado, se crea, esto se hace
                    // solo una vez
                    if (ft.TcpClient == null)
                    {
                        ft.BuildTcpClient();
                    }

                    // Si el tipo de mensaje para reenviar es de tipo cadena
                    if (ft.MessageType == ForwardMessageType.String)
                    {
                        string message = tcpEvent.RawString;
                        // Si el tipo de microservicio cliente es nestjs, entonces el mensaje se pasa a el formato que acepta
                        // nestjs
                        if (ft.Client.GetClientType() == MicroserviceClientType.Nestjs)
                        {
                            message = BuildNestjsMessage(message);
                        }

                        // Si el cliente tcp (que retransmite los mensajes al microservicio)
                        // esta conectado
                        if (ft.TcpClient.Connected == true)
                        {
                            ft.TcpClient.SendMessage(message);
                        }
                    }
                    else if (ft.MessageType == ForwardMessageType.ByteArray)
                    {
                        byte[] rawData = tcpEvent.RawData;
                        // Si el tipo de microservicio cliente es nestjs, entonces el mensaje se pasa a el formato que acepta
                        // nestjs
                        if (ft.Client.GetClientType() == MicroserviceClientType.Nestjs)
                        {
                            // De alguna manera se debe de especificar que los mensajes
                            // los restransmita en base64
                            string encodedMessage = Convert.ToBase64String(rawData);
                            encodedMessage = BuildNestjsMessage(encodedMessage);
                            // Si el cliente tcp (que retransmite los mensajes al microservicio)
                            // esta conectado
                            if (ft.TcpClient.Connected == true)
                            {
                                ft.TcpClient.SendMessage(encodedMessage);
                            }
                        }
                        else
                        {
                            // Si el cliente esta conectado
                            if (ft.TcpClient.Connected == true)
                            {
                                ft.TcpClient.SendMessage(rawData);
                            }
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


        /// <summary>
        /// Evento que sucede cuando un microservicio cliente se desconecta
        /// </summary>
        /// <param name="source">Objeto fuente que origino el evento</param>
        /// <param name="disconnectedEventArgs"></param>
        /// <returns></returns>
        private async Task OnMicroserviceClientDisconnect(object source, MicroserviceDisconnectedEventArgs disconnectedEventArgs)
        {
            TcpServerExecManager tcpServerExecManager = this.tcpServerExecManager.FirstOrDefault(tsm => tsm.MicroserviceId == disconnectedEventArgs.ClientId);
            tcpServerExecManager.StopServerAndAbortThread();
            this.usedPorts.Remove(tcpServerExecManager.TcpPort);
            this.tcpServerExecManager.Remove(tcpServerExecManager);
        }
    }

    public class TcpServerExecManager
    {
        private int microserviceId;

        public int MicroserviceId
        {
            get { return this.microserviceId; }
        }

        private Thread thread;

        public Thread Thread
        {
            get { return this.thread; }
        }

        private TcpServer tcpServer;

        public TcpServer TcpServer
        {
            get { return this.tcpServer; }
        }

        private int tcpPort;

        public int TcpPort
        {
            get { return this.tcpPort; }
        }

        public TcpServerExecManager(int microserviceId, Thread thread, TcpServer tcpServer)
        {
            this.microserviceId = microserviceId;
            this.thread = thread;
            this.tcpServer = tcpServer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="microserviceId"></param>
        /// <param name="thread"></param>
        public TcpServerExecManager(int microserviceId, Thread thread, int tcpPort)
        {
            this.microserviceId = microserviceId;
            this.thread = thread;
            this.tcpPort = tcpPort;
        }

        /// <summary>
        /// Detiene el servidor tcp y aborta el hilo de ejecucion
        /// </summary>
        public void StopServerAndAbortThread()
        {
            this.tcpServer.Stop();
            this.thread.Interrupt();
        }

        /// <summary>
        /// Establece el servidor tcp
        /// </summary>
        /// <param name="tcpServer"></param>
        public void SetTcpServer(TcpServer tcpServer)
        {
            this.tcpServer = tcpServer;
        }

    }

}