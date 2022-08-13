using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Magals.DevicesControl.SDKStandart.Attributes
{
  /// <summary>
  /// Used to set a label on the classes that will have control over ethernet-connect
  /// and for which you will need to create a separate object in memory during configuration
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  [DataContract]
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
    [DataMember]
    public string ip { get; set; } = "0.0.0.0";
    [Required]
    [DataMember]
    public int port { get; set; }
  }
}
