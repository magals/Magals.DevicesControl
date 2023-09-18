using CameraDetectCarNumber_UNV.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CameraDetectCarNumber_UNV
{
    internal class Driver
    {
        private readonly string _ip;
        private readonly int _port;
        private readonly TcpListener server = null;
        public event Action<string> DetectCarNumber;
        private ILogger _logger;

        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken stoppingToken;

        public Driver(string ip, int port, ILogger logger = null)
        {
            _ip = ip;
            _port = port;
            _logger = logger;
            IPAddress localAddr = IPAddress.Parse(_ip);
            server = new TcpListener(localAddr, _port);

        }

        public void StartTcpListener()
        {
            _logger?.LogInformation("StartTcpListener ip:{0} port:{1}", _ip, _port);
            stoppingToken = source.Token;
            ThreadPool.QueueUserWorkItem(StartThreadTcpListener);
        }

        private void StartThreadTcpListener(object o)
        {
            try
            {
                server?.Start();
                Byte[] bytes = new Byte[256];
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger?.LogInformation("Waiting for a connection... ");
                    TcpClient client = server.AcceptTcpClient();
                    _logger?.LogInformation("Connected!");
                    NetworkStream stream = client.GetStream();

                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        try
                        {
                            var cpa = CameraPackage.ParseByteArray(bytes);

                            if (cpa.CodeResult == CodeResult.DetectCar)
                            {
                                DetectCarNumber?.Invoke(cpa.ViewData);
                                _logger?.LogInformation("Received: {0}", cpa.ViewData);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError("Nope data: {0}", ex);
                        }
                        bytes = new Byte[256];
                    }
                }
            }
            catch (SocketException e)
            {
                _logger?.LogError("SocketException: {0}", e);
            }
            finally
            {
                source.Cancel();
                server?.Stop();
            }
        }

        public void StopTcpListener()
        {
            _logger?.LogInformation("StopTcpListener ip:{0} port:{1}", _ip, _port);
            source.Cancel();
            server.Stop();
        }


    }
}
