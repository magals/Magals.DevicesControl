using System;

namespace Magals.DevicesControl.SDKStandart.Attributes
{
    /// <summary>
    /// At runtime, set the configuration name for objects that have the DriverAttribute attribute
    /// </summary>
    public class ConfigNameAttribute : Attribute
    {
        public ConfigNameAttribute(string name, bool enable)
        {
            this.name = name;
            this.enable = enable;
        }

        public string name { get; set; }
        public bool enable { get; set; }
    }
}
