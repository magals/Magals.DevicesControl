using System;
using System.ComponentModel.DataAnnotations;
using System.IO.Ports;

namespace Magals.DevicesControl.SDKStandart.Attributes
{
    /// <summary>
    /// Used to set a label on the classes that will have control over the serial port
    /// and for which you will need to create a separate object in memory during configuration
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SerialPortSettingsAttribute : Attribute
    {
        public bool FlagDefault = true;
        public SerialPortSettingsAttribute()
        {
            this.portname = "COM1";
            this.baudrate = 9600;
        }
        public SerialPortSettingsAttribute(string portname, 
                                           int baudRate, 
                                           Parity parity = Parity.None, 
                                           int dataBits = 8, 
                                           StopBits stopBits = StopBits.One)
        {
            this.portname = portname;
            this.baudrate = baudRate;
            this.parity = parity;
            this.dataBits = dataBits;
            this.stopbits = stopBits;
            if (stopBits == StopBits.None)
            {
                this.stopbits = StopBits.One;
            }
            FlagDefault = false;
        }

        [Required]
        public string portname { get; set; } = "COM1";

        [Required]
        public int baudrate { get; set; } = 9600;
        public Parity parity { get; set; } = Parity.None;
        public int dataBits { get; set; } = 8;
        public StopBits stopbits { get; set; } = StopBits.One;
    }
}
