//-----------------------------------------------------------------------
// <copyright file="MsSql2008StrategyTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Data;
using dbullet.core.dbo;
using dbullet.core.engine;
using dbullet.core.engine.MsSql;
using dbullet.core.exception;
using dbullet.core.test.tools;
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
		[TestMethod]
		[ExpectedException(typeof(CollumnExpectedException))]
		public void CreateTableWithoutCollumnsTest()
		{
			var target = new MsSql2008Strategy(new TestConnection());
			var table = new Table("TestTable");
			target.CreateTable(table);
		}

		/// <summary>
		/// Нормальное создание
		/// </summary>
		[TestMethod]
		public void CreateTable()
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
		public void CreateTableCustomPartition()
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
		public void CreateTableCustomPartitionWithPrimaryKey()
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
		public void CreateTableWithPrimaryKeyCustomPartition()
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
		/// Если строка без размера - сгенерить ошибкуx
		/// </summary>
		[TestMethod]
		public void BuildColumnCreateCommandStringWithoutSizeTest()
		{
			AssertHelpers.Throws<ArgumentException>(
				() => MsSqlTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.String)),
				"String must have length");
		}

		/// <summary>
		/// Обычная колонка-строка
		/// </summary>
		[TestMethod]
		public void BuildColumnCreateCommandStringTest()
		{
			var t = MsSqlTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.String.Size(50)));
			Assert.AreEqual("TestColumn nvarchar(50) null", t);
		}

		/// <summary>
		/// Обычная колонка-число
		/// </summary>
		[TestMethod]
		public void BuildColumnCreateCommandNumericTest()
		{
			var t = MsSqlTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.Decimal.Size(10, 5)));
			Assert.AreEqual("TestColumn decimal(10, 5) null", t);
		}

		/// <summary>
		/// Обычная колонка целое число
		/// </summary>
		[TestMethod]
		public void BuildColumnCreateCommandIntTest()
		{
			var t = MsSqlTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.Int32));
			Assert.AreEqual("TestColumn int null", t);
		}

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[TestMethod]
		public void IsTableExistsTxtTest()
		{
			var connection = new TestConnection { ExecuteScalarValue = 1 };
			var strategy = new MsSql2008Strategy(connection);
			strategy.IsTableExist("ExistingTable");
			Assert.AreEqual(
				"select count(*) from sysobjects " +
				"where id = object_id(N'ExistingTable') and OBJECTPROPERTY(id, N'IsTable') = 1",
				connection.LastCommandText);
		}

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[TestMethod]
		public void IsTableExistsTest()
		{
			var strategy = new MsSql2008Strategy(new TestConnection { ExecuteScalarValue = 1 });
			var actual = strategy.IsTableExist("ExistingTable");
			Assert.AreEqual(true, actual);
		}

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void IsTableExistsEmptyTableTest()
		{
			var strategy = new MsSql2008Strategy(new TestConnection { ExecuteScalarValue = 1 });
			strategy.IsTableExist(string.Empty);
		}

		/// <summary>
		/// Таблица не существует
		/// </summary>
		[TestMethod]
		public void IsTableExistsNotExists()
		{
			var strategy = new MsSql2008Strategy(new TestConnection { ExecuteScalarValue = 0 });
			var actual = strategy.IsTableExist("ExistingTable");
			Assert.AreEqual(false, actual);
		}

		/// <summary>
		/// Удаление таблицы без названия
		/// </summary>
		[TestMethod]
		public void DropEmptyTable()
		{
			var strategy = new MsSql2008Strategy(new TestConnection());
			AssertHelpers.Throws<TableExpectedException>(() => strategy.DropTable(string.Empty));
		}

		/// <summary>
		/// Удаление таблицы
		/// </summary>
		[TestMethod]
		public void DropTable()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.DropTable("TableForDrop");
			Assert.AreEqual("drop table TableForDrop", connection.LastCommandText);
		}

		/// <summary>
		/// Создание индекса
		/// </summary>
		[TestMethod]
		public void CreateIndex()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }));
			Assert.AreEqual("create nonclustered index INDEX_NAME on TABLE_NAME (column4index asc) with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", connection.LastCommandText);
		}

		/// <summary>
		/// Создание индекса оп убыванию
		/// </summary>
		[TestMethod]
		public void CreateIndexDesc()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index", Direction.Descending) }));
			Assert.AreEqual("create nonclustered index INDEX_NAME on TABLE_NAME (column4index desc) with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", connection.LastCommandText);
		}

		/// <summary>
		/// Создание индекса в нестандартной партиции
		/// </summary>
		[TestMethod]
		public void CreateIndexPartitional()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }, "INDEX_PARTITION"));
			Assert.AreEqual("create nonclustered index INDEX_NAME on TABLE_NAME (column4index asc) with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX_PARTITION]", connection.LastCommandText);
		}

		/// <summary>
		/// Создание индекса в нестандартной партиции
		/// </summary>
		[TestMethod]
		public void CreateIndexClustered()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }, "PRIMARY", IndexType.Clustered));
			Assert.AreEqual("create clustered index INDEX_NAME on TABLE_NAME (column4index asc) with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", connection.LastCommandText);
		}

		/// <summary>
		/// Создание уникального индекса
		/// </summary>
		[TestMethod]
		public void CreateIndexUnique()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }, "PRIMARY", IndexType.Nonclustered, true));
			Assert.AreEqual("create unique nonclustered index INDEX_NAME on TABLE_NAME (column4index asc) with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", connection.LastCommandText);
		}

		/// <summary>
		/// Удаление таблицы
		/// </summary>
		[TestMethod]
		public void DropIndex()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.DropIndex(new Index("INDEX_NAME", "TABLE_NAME"));
			Assert.AreEqual("drop index INDEX_NAME on TABLE_NAME", connection.LastCommandText);
		}

		/// <summary>
		/// Создание внешнего ключа
		/// </summary>
		[TestMethod]
		public void CreateForeignKey()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateForeignKey(new ForeignKey("FK_TEST", "TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2"));
			Assert.AreEqual("alter table TABLE1 add constraint FK_TEST foreign key (ID_TABLE1) references TABLE2 (ID_TABLE2) on update no action on delete set null", connection.LastCommandText);
		}

		/// <summary>
		/// Создание внешнего ключа имя автогенерируемое
		/// </summary>
		[TestMethod]
		public void CreateForeignKeyDefaultName()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateForeignKey(new ForeignKey("TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2"));
			Assert.AreEqual("alter table TABLE1 add constraint FK_TABLE1_TABLE2 foreign key (ID_TABLE1) references TABLE2 (ID_TABLE2) on update no action on delete set null", connection.LastCommandText);
		}

		/// <summary>
		/// Создание внешнего ключа имя автогенерируемое
		/// </summary>
		[TestMethod]
		public void CreateForeignKeyCascade()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateForeignKey(new ForeignKey("FK_TEST", "TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2", ForeignAction.Cascade));
			Assert.AreEqual("alter table TABLE1 add constraint FK_TEST foreign key (ID_TABLE1) references TABLE2 (ID_TABLE2) on update no action on delete cascade", connection.LastCommandText);
		}

		/// <summary>
		/// Создание внешнего ключа имя автогенерируемое
		/// </summary>
		[TestMethod]
		public void CreateForeignKeyNoAction()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateForeignKey(new ForeignKey("FK_TEST", "TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2", ForeignAction.NoAction));
			Assert.AreEqual("alter table TABLE1 add constraint FK_TEST foreign key (ID_TABLE1) references TABLE2 (ID_TABLE2) on update no action on delete no action", connection.LastCommandText);
		}

		/// <summary>
		/// Удаление внешнего ключа
		/// </summary>
		[TestMethod]
		public void DropForeignKey()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.DropForeignKey(new ForeignKey("FK_TEST", "TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2", ForeignAction.NoAction));
			Assert.AreEqual("alter table TABLE1 drop constraint FK_TEST", connection.LastCommandText);
		}

		/// <summary>
		/// Нормальное создание
		/// </summary>
		[TestMethod]
		public void CreateTableWithDefault()
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
