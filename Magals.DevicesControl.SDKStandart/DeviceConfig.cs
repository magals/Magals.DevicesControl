using System.Linq;

namespace Magals.DevicesControl.SDKStandart
{
    public static class DeviceConfig
    {
        public static A GetSettingsFromAttribute<A>(object _class)
        {
            var objectAttribute = _class.GetType().GetCustomAttributes(true)
                                                    .Where(a => a.GetType() == typeof(A))
                                                    .Select(a =>
            {
                return (A)a;
            }).FirstOrDefault();
            return objectAttribute;
        }
    }
}
