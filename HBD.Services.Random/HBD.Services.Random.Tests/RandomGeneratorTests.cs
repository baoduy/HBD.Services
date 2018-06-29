using System;
using System.Data;
using HBD.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Services.Random.Tests
{
    [TestClass]
    public class RandomGeneratorTests
    {
        [TestMethod]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void StringTest()
        {
            Assert.AreEqual(12, RandomGenerator.String(12).Length);
            Assert.IsTrue(RandomGenerator.String().Length <= byte.MaxValue);
        }

        [TestMethod]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void BooleanTest()
        {
            Assert.IsInstanceOfType(RandomGenerator.Boolean(), typeof(bool));
        }

        [TestMethod]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void IntTest()
        {
            Assert.IsTrue(RandomGenerator.Int() >= int.MinValue + 1);
            Assert.IsTrue(RandomGenerator.Int() <= int.MaxValue - 1);
            Assert.IsTrue(RandomGenerator.Int(0, 5) <= 5);
            Assert.IsTrue(RandomGenerator.Int(0, 5) >= 0);
            Assert.IsTrue(RandomGenerator.Int(-1) <= -1);
            Assert.IsTrue(RandomGenerator.Int(10, 1) == 10);
        }

        [TestMethod]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void DecimalTest()
        {
            Assert.IsTrue(RandomGenerator.Decimal() >= double.MinValue);
            Assert.IsTrue(RandomGenerator.Decimal() <= double.MaxValue);
            Assert.IsTrue(RandomGenerator.Decimal(0, 500) <= 500);
            Assert.IsTrue(RandomGenerator.Decimal(0, 500) >= 0);
            Assert.IsTrue(RandomGenerator.Decimal(-1) <= -1);
        }

        [TestMethod]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void DateTimeTest()
        {
            Assert.IsTrue(RandomGenerator.DateTime() >= DateTime.MinValue);
            Assert.IsTrue(RandomGenerator.DateTime() <= DateTime.MaxValue);
            Assert.IsTrue(RandomGenerator.DateTime(DateTime.Today.AddDays(1)) <= DateTime.Today.AddDays(1));
            Assert.IsTrue(RandomGenerator.DateTime(DateTime.Today.AddDays(-10), DateTime.Today.AddDays(10)) >=
                          DateTime.Today.AddDays(-10));
            Assert.IsTrue(RandomGenerator.DateTime(DateTime.Today.AddDays(-10), DateTime.Today.AddDays(10)) <=
                          DateTime.Today.AddDays(10));
            Assert.IsTrue(RandomGenerator.DateTime(DateTime.Today.AddDays(10), DateTime.Today.AddDays(-10)) ==
                          DateTime.Today.AddDays(10));
        }

        [TestMethod]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void ByteArrayTest()
        {
            Assert.IsTrue(RandomGenerator.ByteArray().Length <= byte.MaxValue);
            Assert.IsTrue(RandomGenerator.ByteArray(12).Length <= 12);
        }

        [TestMethod]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void DataTableTest()
        {
            using (var data = RandomGenerator.DataTable())
            {
                Assert.IsTrue(data.Columns.Count > 1);
                Assert.IsTrue(data.Rows.Count > 1);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void DataTable_Null_Test()
        {
            using (var data = RandomGenerator.DataTable(null, -1))
            {
                Assert.IsTrue(data.Columns.Count > 1);
                Assert.IsTrue(data.Rows.Count > 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void DataTable_NotNull_Test()
        {
            using (var data = new DataTable())
            {
                data.Columns.AddAutoIncrement("ID");
                data.Columns.Add("Name", typeof(string));

                RandomGenerator.DataTable(data, numberOfRows: 100);
                Assert.AreEqual(100, data.Rows.Count);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void DataTable_100Rows_Test()
        {
            using (var data = RandomGenerator.DataTable(new DataTable(), numberOfRows: 100))
            {
                Assert.IsTrue(data.Columns.Count > 1);
                Assert.IsTrue(data.Rows.Count == 100);
            }
        }

       
    }
}
