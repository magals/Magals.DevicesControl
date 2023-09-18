using CameraDetectCarNumber_UNV.Enums;
using Magals.DevicesControl.SDKStandart.Attributes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CameraDetectCarNumber_UNV
{
    internal class Driver
    {
        private readonly TcpSettingsAttribute settingsTCP;
        private readonly TcpListener server = null;
        public event Action<string> DetectCarNumber;

        public Driver(TcpSettingsAttribute settingsTCP)
        {
            this.settingsTCP = settingsTCP;

            Int32 port = this.settingsTCP.port ;
            IPAddress localAddr = IPAddress.Parse(this.settingsTCP.ip);
            server = new TcpListener(localAddr, port);
        }

        public void StartTcpListener()
        {
            Task.Run(() =>
            {
                try
                {
                    Byte[] bytes = new Byte[256];
                    while (true)
                    {
                        Console.Write("Waiting for a connection... ");
                        TcpClient client = server.AcceptTcpClient();
                        Console.WriteLine("Connected!");
                        NetworkStream stream = client.GetStream();

                        int i;
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            try
                            {
                                var cpa = CameraPackage.ParseByteArray(bytes);

                                if (cpa.CodeResult == CodeResult.DetectCar)
                                {
                                    Console.WriteLine("Received: {0}", cpa.ViewData);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Nope data: ");
                            }
                            bytes = new Byte[256];
                        }
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
                finally
                {
                    server?.Stop();
                }
            });
        }

        public void StopTcpListener()
        {
            server.Stop();
        }


    }
}
