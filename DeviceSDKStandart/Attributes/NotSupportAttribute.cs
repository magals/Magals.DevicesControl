using System;

namespace Magals.DevicesControl.SDKStandart.Attributes
{
    /// <summary>
    /// For methods that need to be implemented by the interface but are not supported by the hardware. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class NotSupportAttribute : Attribute
    {
    }
}
