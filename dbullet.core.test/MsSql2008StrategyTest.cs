//-----------------------------------------------------------------------
// <copyright file="MsSql2008StrategyTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
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
		/// Нормальное создание в другой партиции
		/// </summary>
		[HostType("Moles")]
		[TestMethod]
		public void CreateTableCustomPartition()
		{
			string cmd = string.Empty;
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteNonQuery = p =>
			{
				Assert.AreEqual("create table TestTable (test int null, test2 nvarchar(50) null) on [TESTPARTIOTION]", p.CommandText);
				return 0;
			};
			MSqlCommand.AllInstances.CommandTextSetString = (p, r) => { cmd = r; };
			MSqlCommand.AllInstances.CommandTextGet = p => { return cmd; };
			var target = new MsSql2008Strategy(new MSqlConnection());
			var table = new Table(
				"TestTable", "TESTPARTIOTION", new List<Column> { new Column("test", DbType.Int32), new Column("test2", DbType.String.Size(50)) });
			target.CreateTable(table);
		}

		/// <summary>
		/// Нормальное создание в другой партиции c первичным ключем
		/// </summary>
		[HostType("Moles")]
		[TestMethod]
		public void CreateTableCustomPartitionWithPrimaryKey()
		{
			/*string cmd = string.Empty;
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteNonQuery = p =>
			{
				Assert.AreEqual("create table TestTable (test int null, test2 nvarchar(50) null) on [TESTPARTIOTION]", p.CommandText);
				return 0;
			};
			MSqlCommand.AllInstances.CommandTextSetString = (p, r) => { cmd = r; };
			MSqlCommand.AllInstances.CommandTextGet = p => { return cmd; };
			var target = new MsSql2008Strategy(new MSqlConnection());
			var table = new Table(
				"TestTable", new Partition("TESTPARTIOTION"), new List<Column> { new Column("test", DbType.Int32), new Column("test2", DbType.String.Size(50)) });
			target.CreateTable(table);*/
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

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void IsTableExistsTxtTest()
		{
			string cmd = string.Empty;
			MSqlCommand.AllInstances.ExecuteNonQuery = p =>
			{
				Assert.AreEqual(
					"select count(*) from sysobjects " +
					"where id = object_id(N'ExistingTable') and OBJECTPROPERTY(id, N'IsTable') = 1",
					p.CommandText);
				return 0;
			};
			MSqlCommand.AllInstances.CommandTextSetString = (p, r) => { cmd = r; };
			MSqlCommand.AllInstances.CommandTextGet = p => { return cmd; };
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteScalar = p => { return 1; };
			var strategy = new MsSql2008Strategy(new MSqlConnection());
			strategy.IsTableExist("ExistingTable");
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
	}
}
