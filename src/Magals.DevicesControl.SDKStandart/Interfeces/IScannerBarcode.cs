using System;
using System.Collections.Generic;
using System.Text;

namespace Magals.DevicesControl.SDKStandart.Interfeces
{
    public interface IScannerBarcode : IDevice
    {
        event Action<string> DetectData;
    }
}
