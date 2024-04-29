namespace Magals.DevicesControl.SDKStandart.Enums
{
    public enum BankTerminalCommand
    {
        None = 0,
        Payment = 1,
        Refund = 2,
        Cancel = 3,
        CancelACD = 4,
        ZReport = 5,
    }

    public enum BankTerminalResultCommand
    {
        None = 0,
        Ok = 1,
        Cancel = 2,
        Error = 3,
    }
}
