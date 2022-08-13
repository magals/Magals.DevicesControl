using System;

namespace Magals.DevicesControl.SDKStandart.Models
{
    [Serializable]
    public class PaperStatus : EventArgs
    {
        bool Low { get; set; }
    }
}
