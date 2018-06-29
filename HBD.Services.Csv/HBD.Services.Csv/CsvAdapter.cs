#region using

using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using HBD.Framework;
using HBD.Framework.Data.Base;
using HBD.Framework.Data.GetSetters;

#endregion

namespace HBD.Services.Csv
{
    public class CsvAdapter : DataFileAdapterBase
    {
        public CsvAdapter(string documentFile) : base(documentFile)
        {
        }

        protected override void Dispose(bool isDisposing) { }

        public override void Save()
        {
        }

        public override IGetSetterCollection Read(bool firstRowIsHeader = true)
            => Read(new Services.Csv.ReadCsvOption { FirstRowIsHeader = firstRowIsHeader });

        public virtual IGetSetterCollection Read(Services.Csv.ReadCsvOption option)
        {
            var list = new List<IGetSetter>();
            IGetSetter header = null;

            using (var reader = new CsvReader(File.OpenText(this.DocumentFile), new CsvHelper.Configuration.Configuration { Delimiter = option.Delimiter }))
            {
                //Ignore bad records.
                reader.Configuration.BadDataFound = null;

                if (option.FirstRowIsHeader)
                {
                    reader.Read();
                    reader.ReadHeader();
                    
                    header = new ArrayGetSetter(reader.Context.HeaderRecord);
                }

                while (reader.Read())
                    list.Add(new ArrayGetSetter(reader.Context.Record));
            }

            return new ArrayGetSetterCollection(header, list);
        }

        public override void Write(IGetSetterCollection data, bool ignoreHeader = false)
            => Write(data, new Services.Csv.WriteCsvOption { IgnoreHeader = ignoreHeader });

        public virtual void Write(IGetSetterCollection data, Services.Csv.WriteCsvOption option)
        {
            using (var writer = new CsvWriter(File.CreateText(this.DocumentFile), new CsvHelper.Configuration.Configuration { Delimiter = option.Delimiter }))
            {
                if (!option.IgnoreHeader && data.Header != null)
                {
                    foreach (var item in data.Header)
                        writer.WriteField(item);
                }
                writer.NextRecord();

                foreach (var row in data)
                {
                    foreach (var item in row)
                    {
                        var val = item;

                        //Apply NumericFormat before write
                        if (option.NumericFormat.IsNotNullOrEmpty() && (item.IsNotNumericType() || item.IsNumber()))
                            val = string.Format(option.NumericFormat, item);

                        //Apply DateFormat before write
                        if (option.DateFormat.IsNotNullOrEmpty() && (item is DateTime || item is DateTimeOffset))
                            val = string.Format(option.DateFormat, item);

                        writer.WriteField(val);
                    }

                    writer.NextRecord();
                }
            }
        }
    }
}