using Magals.DevicesControl.SDKStandart.Enums;
using System;
using System.Data;

namespace Magals.DevicesControl.SDKStandart
{
    public static class DataTableArt
    {
        private static class Variable
        {
            public const string id = "id";
            public const string title = "title";
            public const string price = "price";
            public const string quantity = "quantity";
            public const string typeTax = "typeTax";
            public const string discountProcent = "discountProcent";
            public const string discountValue = "discountValue";
        }

        public static DataTable MakeArtTable()
        {
            DataTable namesTable = new DataTable("Arts");

            DataColumn idColumn = new DataColumn();
            idColumn.DataType = typeof(int);
            idColumn.ColumnName = Variable.id;
            idColumn.AutoIncrement = true;
            namesTable.Columns.Add(idColumn);

            DataColumn title = new DataColumn();
            title.DataType = typeof(string);
            title.ColumnName = Variable.title;
            namesTable.Columns.Add(title);

            DataColumn price = new DataColumn();
            price.DataType = typeof(int);
            price.ColumnName = Variable.price;
            namesTable.Columns.Add(price);

            DataColumn qty = new DataColumn();
            qty.DataType = typeof(int);
            qty.ColumnName = Variable.quantity;
            namesTable.Columns.Add(qty);

            DataColumn typeVAT = new DataColumn();
            typeVAT.DataType = typeof(TaxTypes);
            typeVAT.ColumnName = Variable.typeTax;
            namesTable.Columns.Add(typeVAT);

            DataColumn discountProcent = new DataColumn();
            discountProcent.DataType = typeof(long);
            discountProcent.ColumnName = Variable.discountProcent;
            discountProcent.DefaultValue = 0;
            namesTable.Columns.Add(discountProcent);

            DataColumn discountValue = new DataColumn();
            discountValue.DataType = typeof(long);
            discountValue.ColumnName = Variable.discountValue;
            discountValue.DefaultValue = 0;
            namesTable.Columns.Add(discountValue);

            DataColumn[] keys = new DataColumn[1];
            keys[0] = idColumn;
            namesTable.PrimaryKey = keys;

            return namesTable;
        }

        public static void AddRow(ref DataTable table, 
                                  string title, 
                                  long price, 
                                  long quantity, 
                                  TaxTypes typeVAT, 
                                  int discountProcent, 
                                  int discountValue)
        {
            DataRow row = table.NewRow();
            row[Variable.title] = title;
            row[Variable.price] = price;
            row[Variable.quantity] = quantity;
            row[Variable.typeTax] = typeVAT;
            row[Variable.discountProcent] = discountProcent;
            row[Variable.discountValue] = discountValue;
            table.Rows.Add(row);
        }

        public static (string title,
                       long price,
                       long quantity,
                       TaxTypes typeVAT,
                       int discountProcent,
                       int discountValue) 
            GetDataFromRow(DataRow row)
        {
            var title         = row[Variable.title].ToString();
            var price           = Convert.ToInt32(row[Variable.price]);
            var quantity        = Convert.ToInt32(row[Variable.quantity]);
            var typeVAT     = (TaxTypes)Enum.ToObject(typeof(TaxTypes), row[Variable.typeTax]);
            var discountProcent = Convert.ToInt32(row[Variable.discountProcent]);
            var discountValue   = Convert.ToInt32(row[Variable.discountValue]);

            return (title, price, quantity, typeVAT, discountProcent, discountValue);
        }
    }
}
