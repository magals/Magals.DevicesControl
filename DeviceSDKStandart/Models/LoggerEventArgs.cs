using Microsoft.Extensions.Logging;
using System;

namespace Magals.DevicesControl.SDKStandart
{
    [Serializable]
    public class LoggerEventArgs : EventArgs
    {
        public DateTime time { get; }

        public LogLevel eventType { get; }

        public string message { get; }

        public LoggerEventArgs(LogLevel eventtype, string mesg)
        {
            time = DateTime.Now;
            message = mesg;
            eventType = eventtype;
        }
    }
}
