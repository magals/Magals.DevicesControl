using System;
using System.Collections.Generic;
using System.Text;

namespace Magals.DevicesControl.SDKStandart.Interfeces
{
    public interface IBillDispenser : IDevice
    {
        void Enable();
        void Disable();

        void Dispense(int indexCase, int count, int amount);
        Dictionary<string, int> GetStatistics();
        void ResetStatistics();
        void EmptyStored();

        event Action<(int indexCase, int denomination, string currency, int codecurrency)> NoteOutEscrow;
        event Action<(int denomination, string currency, int codecurrency)> NoteInTrash;

        event Action<int> OpenCase;
    }
}
