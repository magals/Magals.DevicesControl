
using Magals.DevicesControl.SDKStandart.Enums;
using System;
using System.Collections.Generic;
using System.Data;

namespace Magals.DevicesControl.SDKStandart.Interfeces
{
    public interface IFiscalPrinter : IDevice
    {
        void OpenReceipt();
        void CloseReceipt();
        void OpenNonFiscalReceipt();
        void CloseNonFiscalReceipt();
        void Subtotal(decimal sum);
        void Payment(TypePayments typePayments, long amount);

        event EventHandler<PaperStatus> eventLentaLow;

        void SetDateTime(DateTime dateTime);
        void SetVAT(string password, Dictionary<TaxTypes, decimal> vattypes);
        void SetNameOperator(uint number, string opername, string password);
        void SetPasswordOperator(uint number, string opername, string password, string oldpassword);
        void SetHeader(uint row, string header);
        void SetFooter(uint row, string header);

        void AddMoneyInMemory(int value);
        void RemoveMoneyFromMemory(int value);

        void PrintText(string linestext);
        void PrintFiscalCheck(DataRow linestext);

        void PrintXReport();
        void PrintXReport(DateTime start, DateTime end);
        void PrintXReportFromСontrolTape();

        void PrintZReport();
        void PrintZReport(DateTime start, DateTime end);
        void PrintZReportFromСontrolTape();

        void SetPasswordOperatorOnDriver(string opername, string password);
    }
}
