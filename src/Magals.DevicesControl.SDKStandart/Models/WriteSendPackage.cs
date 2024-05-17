using System;
using System.Collections.Generic;
using System.Text;

namespace Magals.DevicesControl.SDKStandart.Models
{
    /// <summary>
    /// Information about packets saved and sent
    /// </summary>
    public class WriteSendPackage
    {
        /// <summary>
        /// The date the packet was last sent. Register only while the printer is online
        /// </summary>
        public DateTime LastSend = new DateTime();
        /// <summary>
        /// Number of packets sent
        /// </summary>
        public int CountSend;
        /// <summary>
        /// Number of saved packets
        /// </summary>
        public int CountWrite;
    }
}
