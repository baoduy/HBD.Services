using HBD.Framework;
using HBD.Framework.Data;
using System;
using System.Data;

namespace HBD.Services.Csv
{
    public static class Extensions
    {
        #region Methods

        public static DataTable LoadFromCsv(this DataTable @this, string fileName, Action<ReadCsvOption> options = null)
        {
            var op = new ReadCsvOption();
            options?.Invoke(op);

            new CsvAdapter(fileName).Read(op).ToDataTable(@this);
            return @this;
        }

        public static void SaveToCsv(this DataTable @this, string fileName, Action<WriteCsvOption> options = null)
        {
            var op = new WriteCsvOption();
            options?.Invoke(op);

            new CsvAdapter(fileName).Write(@this.CreateGetSetter(), op);
        }

        #endregion Methods
    }
}