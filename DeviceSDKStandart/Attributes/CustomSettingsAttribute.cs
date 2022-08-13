using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Magals.DevicesControl.SDKStandart.Attributes
{
  /// <summary>
  /// Attribute adds additional properties that can be used as variables
  /// in case when communicating with the equipment information is needed, except for the one that answers
  /// for physical connection
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  [DataContract]
  public class CustomSettingsAttribute : Attribute
  {
    public CustomSettingsAttribute()
    {

    }
    public CustomSettingsAttribute(params object[] keys_value)
    {
      customsettings = new Dictionary<string, object>();

      if (keys_value != null &&
          keys_value.Count() > 1)
      {
        for (int i = 0; i < keys_value.Count(); i += 2)
        {
          if (keys_value[i] != null &&
              keys_value[i + 1] != null)
          {
            this.customsettings.Add(keys_value[i].ToString(), keys_value[i + 1]);
          }
        }
      }
    }
    [DataMember]
    public Dictionary<string, object> customsettings { get; set; }
  }
}
