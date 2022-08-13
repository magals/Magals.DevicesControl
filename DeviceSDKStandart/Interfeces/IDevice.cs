using Magals.DevicesControl.SDKStandart.Models;
using System;
using System.Collections.Generic;

namespace Magals.DevicesControl.SDKStandart.Interfeces
{

    public interface IDevice : IDisposable
    {
        bool IsConnected { get; }
        bool EnableAutoScan { get; }

        event EventHandler<LoggerEventArgs> LogMessage;

        /// <summary>
        /// Describe the connection to the device.
        /// IsConnected - set true, if connect to device
        /// </summary>
        /// <returns>Status connect</returns>
        bool Connect();

        /// <summary>
        /// Disconnect from device
        /// set IsConnected - false 
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Getting device status statuses
        /// </summary>
        /// <returns>List statues</returns>
        Dictionary<string, object> GetStatuses();

        /// <summary>
        /// Get a unique number
        /// </summary>
        /// <returns>Number device</returns>
        string GetSerialDevice();

        /// <summary>
        /// Automatic device discovery based on all ports (SerialPorts/TCP ..etc)
        /// </summary>
        void AutoDetectDevice();

        /// <summary>
        /// For cases when it is very necessary to implement a method, but it is not declared in the interface 
        /// </summary>
        /// <param name="custom">Any object</param>
        /// <returns>any object</returns>
        object CustomMethod(object custom);
    }
}
