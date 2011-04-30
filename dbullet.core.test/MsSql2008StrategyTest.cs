using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient.Moles;
using dbullet.core.dbo;
using dbullet.core.dbs;
using dbullet.core.exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test
{
	/// <summary>
	/// Тест MSSQL 2008 стратегии
	/// </summary>
	[TestClass]
	public class MsSql2008StrategyTest
	{
		/// <summary>
		/// Контекст
		/// </summary>
		public TestContext TestContext { get; set; }

		/// <summary>
		/// Создание таблицы без столбцов
		/// </summary>
		[HostType("Moles")]
		[TestMethod]
		[ExpectedException(typeof(CollumnExpectedException))]
		public void CreateTableWithoutCollumnsTest()
		{
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteNonQuery = p => { return 0; };
			var target = new MsSql2008Strategy(new MSqlConnection());
			var table = new Table("TestTable", new List<Column>());
			target.CreateTable(table);
		}

		/// <summary>
		/// Нормальное создание
		/// </summary>
		[HostType("Moles")]
		[TestMethod]
		public void CreateTable()
		{
			string cmd = string.Empty;
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteNonQuery = p =>
				{
					Assert.AreEqual("create table TestTable (test int null, test2 nvarchar(50) null) on [PRIMARY]", p.CommandText);
					return 0;
				};
			MSqlCommand.AllInstances.CommandTextSetString = (p, r) => { cmd = r; };
			MSqlCommand.AllInstances.CommandTextGet = p => { return cmd; };
			var target = new MsSql2008Strategy(new MSqlConnection());
			var table = new Table(
				"TestTable", new List<Column> { new Column("test", DbType.Int32), new Column("test2", DbType.String.Size(50)) });
			target.CreateTable(table);
		}

		/// <summary>
		/// Если строка без размера - сгенерить ошибку
		/// </summary>
		[TestMethod]
		public void BuildColumnCreateCommandStringWithoutSizeTest()
		{
			AssertHelpers.Throws<ArgumentException>(
				() => MsSql2008Strategy.BuildColumnCreateCommand(new Column("TestColumn", DbType.String)),
				"String must have length");
		}

		/// <summary>
		/// Обычная колонка-строка
		/// </summary>
		[TestMethod]
		public void BuildColumnCreateCommandStringTest()
		{
			var t = MsSql2008Strategy.BuildColumnCreateCommand(new Column("TestColumn", DbType.String.Size(50)));
			Assert.AreEqual("TestColumn nvarchar(50) null", t);
		}

		/// <summary>
		/// Обычная колонка-число
		/// </summary>
		[TestMethod]
		public void BuildColumnCreateCommandNumericTest()
		{
			var t = MsSql2008Strategy.BuildColumnCreateCommand(new Column("TestColumn", DbType.Decimal.Size(10, 5)));
			Assert.AreEqual("TestColumn decimal(10, 5) null", t);
		}

		/// <summary>
		/// Обычная колонка целое число
		/// </summary>
		[TestMethod]
		public void BuildColumnCreateCommandIntTest()
		{
			var t = MsSql2008Strategy.BuildColumnCreateCommand(new Column("TestColumn", DbType.Int32));
			Assert.AreEqual("TestColumn int null", t);
		}
	}
}
