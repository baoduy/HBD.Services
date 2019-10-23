using HBD.Framework.Data.GetSetters;
using HBD.Services.Csv.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.IO;

namespace HBD.Services.Csv.Tests
{
    [TestClass]
    public class CsvTests
    {
        #region Methods

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        [ExpectedException(typeof(FileNotFoundException))]
        public void LoadFromCSV_CsvFileNotExisted_Test()
        {
            var tb = new DataTable().LoadFromCsv("TestData\\AAA.csv", op => op.Delimiter = "|");
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void LoadFromCSV_Customers_FirstRowIsHeader_Test()
        {
            var tb = new DataTable();
            tb.LoadFromCsv("TestData\\Northwind\\Customers.csv", op => op.FirstRowIsHeader = true);
            Assert.IsTrue(tb.Columns.Count == 11);
            Assert.IsTrue(tb.Rows.Count == 91);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void LoadFromCSV_Customers_FirstRowIsNotHeader_Test()
        {
            var tb = new DataTable();
            tb.LoadFromCsv("TestData\\Northwind\\Customers.csv", op => op.FirstRowIsHeader = false);
            Assert.IsTrue(tb.Columns.Count == 11);
            Assert.IsTrue(tb.Rows.Count == 92);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void LoadFromCSV_Employees_Test()
        {
            var tb = new DataTable();
            tb.LoadFromCsv("TestData\\Northwind\\Employees_PileDilimiters.csv", op => op.Delimiter = "|");
            Assert.IsTrue(tb.Columns.Count >= 10);
            Assert.IsTrue(tb.Rows.Count >= 9);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadFromCSV_NullDataTable_Test()
        {
            var tb = ((DataTable)null).LoadFromCsv("TestData\\Northwind\\Employees.csv", op => op.Delimiter = "|");
        }

        //[TestMethod]
        //[TestCategory("Fw.DataTableExtensions")]
        //public void SaveTable_ToCsv_Test()
        //{
        //    const string fileName = "TestDataTable_SaveToCsv.csv";
        //    var data = CreateTable();
        //    data.SaveToCsv(fileName);

        //    Assert.IsTrue(File.Exists(fileName));
        //    Assert.IsTrue(File.ReadAllLines(fileName).Length > 0);
        //}

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void SaveObjects_ToCsv_Test()
        {
            var fileName = "TestObjects_SaveToCsv.csv";
            var data = new[]
            {
                new TestItem {Id = 1, Name = "1", Details = "One"},
                new TestItem {Id = 2, Name = "2", Details = "Two"},
                new TestItem {Id = 3, Name = "3", Details = "Three"},
                new TestItem {Id = 4, Name = "4", Details = "Four"},
                new TestItem {Id = 5, Name = "5", Details = "Five"}
            };

            new CsvAdapter(fileName).Write(new ObjectGetSetterCollection<TestItem>(data));

            Assert.IsTrue(File.Exists(fileName));
            Assert.IsTrue(File.ReadAllLines(fileName).Length > 0);
        }

        #endregion Methods
    }
}