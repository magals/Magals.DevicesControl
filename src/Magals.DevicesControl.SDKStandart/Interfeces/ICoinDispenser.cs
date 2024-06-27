using System;
using System.Collections.Generic;
using System.Text;

namespace Magals.DevicesControl.SDKStandart.Interfeces
{
    public interface ICoinDispenser : IDevice
    {
        void DispanseCoin(int denomination, string currency, int amount, bool autoOutDenomination = false );
        Dictionary<string, int> GetInfoStored();
        Dictionary<string, int> GetStatistics();
        void ResetStatistics();
        void EmptyStored();

        event Action<(int denomination, string currency, int codecurrency)> CoinDispensed;
    }
}
