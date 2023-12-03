using System;
using System.Collections.Generic;
using System.Text;

namespace Magals.DevicesControl.SDKStandart.Interfeces.GPIO
{
    public interface IRGBLed : IDevice
    {
        void OnOff(bool red, bool green, bool blue);
    }
}
