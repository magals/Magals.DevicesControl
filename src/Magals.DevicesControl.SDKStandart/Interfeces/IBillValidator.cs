using System;
using System.Collections.Generic;
using System.Text;

namespace Magals.DevicesControl.SDKStandart.Interfeces
{
    public interface IBillValidator : IDevice
    {
        void Enable(string[] allowedDenomination);
        void Disable();

        Dictionary<string, int> GetStatistics();
        void ResetStatistics();
        void EmptyStored();

        event Action<(int denomination, string currency, int codecurrency)> NoteInEscrow;
        event Action<(int denomination, string currency, int codecurrency)> NoteInStored;
        event Action NoteReject;

        event Action<int> OpenCase;
    }
}
