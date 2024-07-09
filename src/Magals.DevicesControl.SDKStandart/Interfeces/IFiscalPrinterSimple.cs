using Magals.DevicesControl.SDKStandart.Enums;
using Magals.DevicesControl.SDKStandart.Models;
using System;

namespace Magals.DevicesControl.SDKStandart.Interfeces
{
    public interface IFiscalPrinterSimple : IDevice
    {
        string SerialNum { get; }
        void Sale(ulong productCode, TaxTypes taxRateGroup, bool productNameFromMemory, string productName, decimal productQuantity, decimal productPrice, bool printProductBarcode);
        void Payment(TypePayments paymentType, decimal paymentAmount, bool isServiceReceipt, bool closeReceipt, ulong productCodeFuel = 255);
        WriteSendPackage GetWriteAndSendPackages();
        void PrintXReport();
        void Comment(string text);
        void PrintZReport(bool printpaper = true);
    }
}
