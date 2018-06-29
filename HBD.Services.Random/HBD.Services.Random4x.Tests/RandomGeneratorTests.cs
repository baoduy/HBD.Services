using System.Data;
using System.Linq;
using HBD.Framework;
using HBD.Framework.Exceptions;
using HBD.Services.Random;
using HBD.Services.Sql.Base;
using HBD.Services.Sql.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Services.Random4x.Tests
{
    [TestClass]
    public class RandomGeneratorTests
    {
        [TestMethod]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void TableInfo_IntPrimary_Test()
        {
            var info = new TableInfo("A", "B");
            info.Columns.Add(new ColumnInfo
            {
                IsPrimaryKey = true,
                Name = "Col1",
                DataType = typeof(int).ToSqlDbType()
            });
            info.Columns.Add(new ColumnInfo
            {
                Name = "Col2",
                DataType = typeof(string).ToSqlDbType()
            });
            info.Columns.Add(new ColumnInfo
            {
                Name = "Col3",
                DataType = typeof(int).ToSqlDbType(),
                IsComputed = true,
                ComputedExpression = "Col1"
            });
            info.Columns.Add(new ColumnInfo
            {
                Name = "Col4",
                DataType = typeof(int).ToSqlDbType(),
                IsIdentity = true
            });

            using (var data = RandomGenerator.TableInfo(info, 100))
            {
                Assert.AreEqual(4, data.Columns.Count);
                Assert.AreEqual(100, data.Rows.Count);

                Assert.AreEqual(data.Rows[0][0], data.Rows[0][2]);
                Assert.IsTrue((int)data.Rows[0][3] >= 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void TableInfo_StringPrimary_Test()
        {
            var info = new TableInfo("A", "B");
            info.Columns.Add(new ColumnInfo
            {
                IsPrimaryKey = true,
                Name = "Col1",
                DataType = typeof(string).ToSqlDbType()
            });
            info.Columns.Add(new ColumnInfo
            {
                Name = "Col2",
                DataType = typeof(string).ToSqlDbType()
            });
            info.Columns.Add(new ColumnInfo
            {
                Name = "Col3",
                DataType = typeof(int).ToSqlDbType(),
                IsIdentity = true
            });
            info.Columns.Add(new ColumnInfo
            {
                Name = "Col4",
                DataType = typeof(int).ToSqlDbType(),
                IsComputed = true,
                ComputedExpression = "Col3"
            });

            using (var data = RandomGenerator.TableInfo(info, 100))
            {
                Assert.AreEqual(4, data.Columns.Count);
                Assert.AreEqual(100, data.Rows.Count);

                Assert.AreEqual(data.Rows[0][3], data.Rows[0][2]);
                Assert.IsTrue((int)data.Rows[0][3] >= 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void TableInfo_StringPrimaryMaxLengh1000_Test()
        {
            var info = new TableInfo("A", "B");
            info.Columns.Add(new ColumnInfo
            {
                IsPrimaryKey = true,
                Name = "Col1",
                DataType = typeof(string).ToSqlDbType(),
                MaxLengh = 1000
            });
            info.Columns.Add(new ColumnInfo
            {
                Name = "Col2",
                DataType = typeof(string).ToSqlDbType()
            });

            using (var data = RandomGenerator.TableInfo(info, 100))
            {
                Assert.AreEqual(2, data.Columns.Count);
                Assert.AreEqual(100, data.Rows.Count);

                Assert.IsTrue(data.Rows.Cast<DataRow>().All(r => r.ItemArray.All(b => b.IsNotNull())));
            }
        }

        [TestMethod]
        [TestCategory("Fw.Testing.RandomGenerator")]
        [ExpectedException(typeof(OutOfCapacityException))]
        public void TableInfo_IntPrimaryMax_Test()
        {
            var info = new TableInfo("A", "B");
            info.Columns.Add(new ColumnInfo
            {
                IsPrimaryKey = true,
                Name = "Col1",
                DataType = typeof(int).ToSqlDbType(),
                MaxPrimaryKeyValue = int.MaxValue
            });

            using (var data = RandomGenerator.TableInfo(info, 100))
            {
            }
        }

        [TestMethod]
        [TestCategory("Fw.Testing.RandomGenerator")]
        [ExpectedException(typeof(OutOfCapacityException))]
        public void TableInfo_StringPrimaryMax_Test()
        {
            var info = new TableInfo("A", "B");
            info.Columns.Add(new ColumnInfo
            {
                IsPrimaryKey = true,
                Name = "Col1",
                DataType = typeof(string).ToSqlDbType(),
                MaxPrimaryKeyValue = "ZZZZ",
                MaxLengh = 4
            });

            using (var data = RandomGenerator.TableInfo(info, 100))
            {
            }
        }
    }
}
