using System;
using System.ComponentModel.DataAnnotations;

namespace Magals.DevicesControl.SDKStandart.Attributes
{
    /// <summary>
    /// Used to set a label on the classes that will have control over ethernet-connect
    /// and for which you will need to create a separate object in memory during configuration
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TcpSettingsAttribute : Attribute
    {
        public bool FlagDefault = true;
        public TcpSettingsAttribute()
        {
            this.ip = "0.0.0.0";
        }

        //Use attribute for set default value in config
        public TcpSettingsAttribute(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            FlagDefault = false;
        }

        [Required]
        public string ip { get; set; } = "0.0.0.0";
        [Required]
        public int port { get; set; }
    }
}
