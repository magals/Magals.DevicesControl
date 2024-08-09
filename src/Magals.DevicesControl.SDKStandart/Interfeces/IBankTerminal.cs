using Magals.DevicesControl.SDKStandart.Enums;
using System;

namespace Magals.DevicesControl.SDKStandart.Interfeces
{
    public interface IBankTerminal : IDevice
    {
        void Payment(long amount, int code_currency, TypePayments typePayments = TypePayments.CARD);
        void Refund(long amount);
        void Cancel(long amount);
        void CancelACD(long amount);
        void ZReport();
        void XReport();

        event Action<(BankTerminalCommand command, BankTerminalResultCommand resultCode, object resultObjects, string stringForPrint)> ActionResultCommand;
        event Action<string> ResultReport;

    }
}
