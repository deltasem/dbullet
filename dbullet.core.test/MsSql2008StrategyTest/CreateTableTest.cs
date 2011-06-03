//-----------------------------------------------------------------------
// <copyright file="CreateTableTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Data;
using dbullet.core.dbo;
using dbullet.core.engine;
using dbullet.core.exception;
using dbullet.core.test.tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// Тесты создания таблиц
	/// </summary>
	[TestClass]
	public class CreateTableTest
	{
		/// <summary>
		/// Создание таблицы без столбцов
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(CollumnExpectedException))]
		public void WithoutCollumns()
		{
			var target = new MsSql2008Strategy(new TestConnection());
			var table = new Table("TestTable");
			target.CreateTable(table);
		}

		/// <summary>
		/// Нормальное создание
		/// </summary>
		[TestMethod]
		public void RegularCreateTable()
		{
			var connection = new TestConnection();
			var target = new MsSql2008Strategy(connection);
			var table = new Table("TestTable");
			table.AddColumn(new Column("test", DbType.Int32));
			table.AddColumn(new Column("test2", DbType.String.Size(50)));
			target.CreateTable(table);
			Assert.AreEqual("create table TestTable (test int null, test2 nvarchar(50) null) on [PRIMARY]", connection.LastCommandText);
		}

		/// <summary>
		/// Нормальное создание в другой партиции
		/// </summary>
		[TestMethod]
		public void CustomPartition()
		{
			var connection = new TestConnection();
			var target = new MsSql2008Strategy(connection);
			var table = new Table("TestTable", "TESTPARTIOTION")
				.AddColumn(new Column("test", DbType.Int32))
				.AddColumn(new Column("test2", DbType.String.Size(50)));
			target.CreateTable(table);
			Assert.AreEqual("create table TestTable (test int null, test2 nvarchar(50) null) on [TESTPARTIOTION]", connection.LastCommandText);
		}

		/// <summary>
		/// Нормальное создание в другой партиции c первичным ключем
		/// </summary>
		[TestMethod]
		public void CustomPartitionWithPrimaryKey()
		{
			var connection = new TestConnection();
			var target = new MsSql2008Strategy(connection);
			var table = new Table(
				"TestTable", "TESTPARTIOTION")
				.AddColumn(new Column("testid", DbType.Int32))
				.AddColumn(new Column("test2", DbType.String.Size(50)))
				.AddPrimaryKey("testid");
			target.CreateTable(table);
			Assert.AreEqual("create table TestTable (testid int null, test2 nvarchar(50) null, constraint PK_TESTTABLE primary key clustered(testid asc) with (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) on [TESTPARTIOTION]", connection.LastCommandText);
		}

		/// <summary>
		/// Создание таблицы с первичным ключем в не стандартной партиции
		/// </summary>
		[TestMethod]
		public void WithPrimaryKeyCustomPartition()
		{
			var connection = new TestConnection();
			var target = new MsSql2008Strategy(connection);
			var table = new Table(
				"TestTable")
				.AddColumn(new Column("testid", DbType.Int32))
				.AddColumn(new Column("test2", DbType.String.Size(50)))
				.AddPrimaryKey("testid", "TESTPARTIOTION");
			target.CreateTable(table);
			Assert.AreEqual("create table TestTable (testid int null, test2 nvarchar(50) null, constraint PK_TESTTABLE primary key clustered(testid asc) with (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [TESTPARTIOTION]) on [PRIMARY]", connection.LastCommandText);
		}

		/// <summary>
		/// Нормальное создание
		/// </summary>
		[TestMethod]
		public void WithDefault()
		{
			var connection = new TestConnection();
			var target = new MsSql2008Strategy(connection);
			var table = new Table("TestTable");
			table.AddColumn(new Column("test", DbType.Int32)).Default("100500");
			table.AddColumn(new Column("test2", DbType.String.Size(50))).Default("this is the test");
			target.CreateTable(table);
			Assert.AreEqual("create table TestTable (test int null constraint DF_TESTTABLE_TEST default '100500', test2 nvarchar(50) null constraint DF_TESTTABLE_TEST2 default 'this is the test') on [PRIMARY]", connection.LastCommandText);
		}
	}
}