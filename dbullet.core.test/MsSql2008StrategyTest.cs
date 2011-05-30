//-----------------------------------------------------------------------
// <copyright file="MsSql2008StrategyTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient.Moles;
using dbullet.core.dbo;
using dbullet.core.engine;
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
			var table = new Table("TestTable");
			target.CreateTable(table);
		}

		/// <summary>
		/// Нормальное создание
		/// </summary>
		[HostType("Moles")]
		[TestMethod]
		public void CreateTable()
		{
			bool executed = false;
			string cmd = string.Empty;
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteNonQuery = p =>
				{
					executed = true;
					Assert.AreEqual("create table TestTable (test int null, test2 nvarchar(50) null) on [PRIMARY]", p.CommandText);
					return 0;
				};
			MSqlCommand.AllInstances.CommandTextSetString = (p, r) => { cmd = r; };
			MSqlCommand.AllInstances.CommandTextGet = p => { return cmd; };
			var target = new MsSql2008Strategy(new MSqlConnection());
			var table = new Table("TestTable");
			table.AddColumn(new Column("test", DbType.Int32));
			table.AddColumn(new Column("test2", DbType.String.Size(50)));
			target.CreateTable(table);
			Assert.IsTrue(executed);
		}

		/// <summary>
		/// Нормальное создание в другой партиции
		/// </summary>
		[HostType("Moles")]
		[TestMethod]
		public void CreateTableCustomPartition()
		{
			bool executed = false;
			string cmd = string.Empty;
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteNonQuery = p =>
			{
				executed = true;
				Assert.AreEqual("create table TestTable (test int null, test2 nvarchar(50) null) on [TESTPARTIOTION]", p.CommandText);
				return 0;
			};
			MSqlCommand.AllInstances.CommandTextSetString = (p, r) => { cmd = r; };
			MSqlCommand.AllInstances.CommandTextGet = p => { return cmd; };
			var target = new MsSql2008Strategy(new MSqlConnection());
			var table = new Table("TestTable", "TESTPARTIOTION")
				.AddColumn(new Column("test", DbType.Int32))
				.AddColumn(new Column("test2", DbType.String.Size(50)));
			target.CreateTable(table);
			Assert.IsTrue(executed);
		}

		/// <summary>
		/// Нормальное создание в другой партиции c первичным ключем
		/// </summary>
		[HostType("Moles")]
		[TestMethod]
		public void CreateTableCustomPartitionWithPrimaryKey()
		{
			bool executed = false;
			string cmd = string.Empty;
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteNonQuery = p =>
			{
				executed = true;
				Assert.AreEqual("create table TestTable (testid int null, test2 nvarchar(50) null, constraint PK_TESTTABLE primary key clustered(testid asc) with (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) on [TESTPARTIOTION]", p.CommandText);
				return 0;
			};
			MSqlCommand.AllInstances.CommandTextSetString = (p, r) => { cmd = r; };
			MSqlCommand.AllInstances.CommandTextGet = p => { return cmd; };
			var target = new MsSql2008Strategy(new MSqlConnection());
			var table = new Table(
				"TestTable", "TESTPARTIOTION")
				.AddColumn(new Column("testid", DbType.Int32))
				.AddColumn(new Column("test2", DbType.String.Size(50)))				
				.AddPrimaryKey("testid");
			target.CreateTable(table);
			Assert.IsTrue(executed);
		}

		/// <summary>
		/// Если строка без размера - сгенерить ошибкуx
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

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void IsTableExistsTxtTest()
		{
			bool executed = false;
			string cmd = string.Empty;
			MSqlCommand.AllInstances.ExecuteScalar = p =>
			{
				executed = true;
				Assert.AreEqual(
					"select count(*) from sysobjects " +
					"where id = object_id(N'ExistingTable') and OBJECTPROPERTY(id, N'IsTable') = 1",
					p.CommandText);
				return 1;
			};
			MSqlCommand.AllInstances.CommandTextSetString = (p, r) => { cmd = r; };
			MSqlCommand.AllInstances.CommandTextGet = p => { return cmd; };
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			var strategy = new MsSql2008Strategy(new MSqlConnection());
			strategy.IsTableExist("ExistingTable");
			Assert.IsTrue(executed);
		}

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void IsTableExistsTest()
		{
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteScalar = p => { return 1; };
			var strategy = new MsSql2008Strategy(new MSqlConnection());
			var actual = strategy.IsTableExist("ExistingTable");
			Assert.AreEqual(true, actual);
		}

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		[ExpectedException(typeof(ArgumentException))]
		public void IsTableExistsEmptyTableTest()
		{
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteScalar = p => { return 1; };
			var strategy = new MsSql2008Strategy(new MSqlConnection());
			strategy.IsTableExist(string.Empty);
		}

		/// <summary>
		/// Таблица не существует
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void IsTableExistsNotExists()
		{
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteScalar = p => { return 0; };
			var strategy = new MsSql2008Strategy(new MSqlConnection());
			var actual = strategy.IsTableExist("ExistingTable");
			Assert.AreEqual(false, actual);
		}

		/// <summary>
		/// Удаление таблицы без названия
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void DropEmptyTable()
		{
			var strategy = new MsSql2008Strategy(new MSqlConnection());
			AssertHelpers.Throws<TableExpectedException>(() => strategy.DropTable(string.Empty));
		}

		/// <summary>
		/// Удаление таблицы
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void DropTable()
		{
			bool executed = false;
			string cmd = string.Empty;
			MSqlCommand.AllInstances.ExecuteNonQuery = p =>
			{
				executed = true;
				Assert.AreEqual("drop table TableForDrop", p.CommandText);
				return 0;
			};
			MSqlCommand.AllInstances.CommandTextSetString = (p, r) => { cmd = r; };
			MSqlCommand.AllInstances.CommandTextGet = p => { return cmd; };
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteScalar = p => { return 1; };
			var strategy = new MsSql2008Strategy(new MSqlConnection());
			strategy.DropTable("TableForDrop");
			Assert.IsTrue(executed);
		}

		/// <summary>
		/// Создание индекса
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void CreateIndex()
		{
			bool executed = false;
			string cmd = string.Empty;
			MSqlCommand.AllInstances.ExecuteNonQuery = p =>
			{
				executed = true;
				Assert.AreEqual("create nonclustered index INDEX_NAME on TABLE_NAME (column4index asc) whth (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", p.CommandText);
				return 0;
			};
			MSqlCommand.AllInstances.CommandTextSetString = (p, r) => { cmd = r; };
			MSqlCommand.AllInstances.CommandTextGet = p => { return cmd; };
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteScalar = p => { return 1; };
			var strategy = new MsSql2008Strategy(new MSqlConnection());
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }));
			Assert.IsTrue(executed);
		}

		/// <summary>
		/// Создание индекса оп убыванию
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void CreateIndexDesc()
		{
			bool executed = false;
			string cmd = string.Empty;
			MSqlCommand.AllInstances.ExecuteNonQuery = p =>
			{
				executed = true;
				Assert.AreEqual("create nonclustered index INDEX_NAME on TABLE_NAME (column4index desc) whth (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", p.CommandText);
				return 0;
			};
			MSqlCommand.AllInstances.CommandTextSetString = (p, r) => { cmd = r; };
			MSqlCommand.AllInstances.CommandTextGet = p => { return cmd; };
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteScalar = p => { return 1; };
			var strategy = new MsSql2008Strategy(new MSqlConnection());
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index", Direction.Descending) }));
			Assert.IsTrue(executed);
		}

		/// <summary>
		/// Создание индекса в нестандартной партиции
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void CreateIndexPartitional()
		{
			bool executed = false;
			string cmd = string.Empty;
			MSqlCommand.AllInstances.ExecuteNonQuery = p =>
			{
				executed = true;
				Assert.AreEqual("create nonclustered index INDEX_NAME on TABLE_NAME (column4index asc) whth (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX_PARTITION]", p.CommandText);
				return 0;
			};
			MSqlCommand.AllInstances.CommandTextSetString = (p, r) => { cmd = r; };
			MSqlCommand.AllInstances.CommandTextGet = p => { return cmd; };
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteScalar = p => { return 1; };
			var strategy = new MsSql2008Strategy(new MSqlConnection());
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }, "INDEX_PARTITION"));
			Assert.IsTrue(executed);
		}

		/// <summary>
		/// Создание индекса в нестандартной партиции
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void CreateIndexClustered()
		{
			bool executed = false;
			string cmd = string.Empty;
			MSqlCommand.AllInstances.ExecuteNonQuery = p =>
			{
				executed = true;
				Assert.AreEqual("create clustered index INDEX_NAME on TABLE_NAME (column4index asc) whth (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", p.CommandText);
				return 0;
			};
			MSqlCommand.AllInstances.CommandTextSetString = (p, r) => { cmd = r; };
			MSqlCommand.AllInstances.CommandTextGet = p => { return cmd; };
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteScalar = p => { return 1; };
			var strategy = new MsSql2008Strategy(new MSqlConnection());
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }, "PRIMARY", IndexType.Clustered));
			Assert.IsTrue(executed);
		}

		/// <summary>
		/// Создание уникального индекса
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void CreateIndexUnique()
		{
			bool executed = false;
			string cmd = string.Empty;
			MSqlCommand.AllInstances.ExecuteNonQuery = p =>
			{
				executed = true;
				Assert.AreEqual("create unique nonclustered index INDEX_NAME on TABLE_NAME (column4index asc) whth (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", p.CommandText);
				return 0;
			};
			MSqlCommand.AllInstances.CommandTextSetString = (p, r) => { cmd = r; };
			MSqlCommand.AllInstances.CommandTextGet = p => { return cmd; };
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteScalar = p => { return 1; };
			var strategy = new MsSql2008Strategy(new MSqlConnection());
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }, "PRIMARY", IndexType.Nonclustered, true));
			Assert.IsTrue(executed);
		}
	}
}
