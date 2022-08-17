using Magals.DevicesControl.SDKStandart.Enums;
using System;

namespace Magals.DevicesControl.SDKStandart.Interfeces
{
    public interface IBankTerminal : IDevice
    {
        void Sale(long amount, string name_currency);
        void Refund(long amount);
        void Cancel(long amount);
        void CancelACD(long amount);
        void ZReport();

        event Action<BankTerminalCommand, BankTerminalResultCommand, object> ActionResultCommand;
    }
}
