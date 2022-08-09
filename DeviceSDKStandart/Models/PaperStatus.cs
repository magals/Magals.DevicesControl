using System;

namespace Magals.DevicesControl.SDKStandart
{
    [Serializable]
    public class PaperStatus : EventArgs
    {
        bool Low { get; set; }
    }
}
