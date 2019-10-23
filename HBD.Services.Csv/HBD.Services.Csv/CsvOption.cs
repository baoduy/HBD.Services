namespace HBD.Services.Csv
{
    public abstract class CsvOption
    {
        #region Properties

        public string Delimiter { get; set; } = ",";

        #endregion Properties
    }

    public class ReadCsvOption : CsvOption
    {
        #region Properties

        public bool FirstRowIsHeader { get; set; } = true;

        #endregion Properties
    }

    public class WriteCsvOption : CsvOption
    {
        #region Properties

        public string DateFormat { get; set; }

        public bool IgnoreHeader { get; set; } = false;

        public string NumericFormat { get; set; }

        #endregion Properties
    }
}