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
			Assert.AreEqual("create table TestTable (test int null, test2 nvarchar(50) null) on [PRIMARY]", connection.LastCommandText.Replace("\r", string.Empty).Replace("\n", string.Empty));
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
			Assert.AreEqual("create table TestTable (test int null, test2 nvarchar(50) null) on [TESTPARTIOTION]", connection.LastCommandText.Replace("\r", string.Empty).Replace("\n", string.Empty));
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
			Assert.AreEqual("create table TestTable (testid int null, test2 nvarchar(50) null, constraint PK_TESTTABLE primary key clustered(testid asc) with (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) on [TESTPARTIOTION]", connection.LastCommandText.Replace("\r", string.Empty).Replace("\n", string.Empty));
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
			Assert.AreEqual("create table TestTable (testid int null, test2 nvarchar(50) null, constraint PK_TESTTABLE primary key clustered(testid asc) with (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [TESTPARTIOTION]) on [PRIMARY]", connection.LastCommandText.Replace("\r", string.Empty).Replace("\n", string.Empty));
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
			Assert.AreEqual("create nonclustered index INDEX_NAME on TABLE_NAME (column4index asc) whth (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", connection.LastCommandText.Replace("\r", string.Empty).Replace("\n", string.Empty));
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
			Assert.AreEqual("create nonclustered index INDEX_NAME on TABLE_NAME (column4index desc) whth (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", connection.LastCommandText.Replace("\r", string.Empty).Replace("\n", string.Empty));
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
			Assert.AreEqual("create nonclustered index INDEX_NAME on TABLE_NAME (column4index asc) whth (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX_PARTITION]", connection.LastCommandText.Replace("\r", string.Empty).Replace("\n", string.Empty));
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
			Assert.AreEqual("create clustered index INDEX_NAME on TABLE_NAME (column4index asc) whth (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", connection.LastCommandText.Replace("\r", string.Empty).Replace("\n", string.Empty));
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
			Assert.AreEqual("create unique nonclustered index INDEX_NAME on TABLE_NAME (column4index asc) whth (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", connection.LastCommandText.Replace("\r", string.Empty).Replace("\n", string.Empty));
		}

		/// <summary>
		/// Загулшка для подключения
		/// </summary>
		public class TestConnection : IDbConnection
		{
			/// <summary>
			/// Состояние
			/// </summary>
			private ConnectionState state;

			/// <summary>
			/// Значение для запроса без результата
			/// </summary>
			public int ExecuteNonQueryValue { get; set; }

			/// <summary>
			/// Последний запрос
			/// </summary>
			public string LastCommandText { get; set; }

			/// <summary>
			/// Значение для ExecuteScalar
			/// </summary>
			public object ExecuteScalarValue { get; set; }

			#region Implementation of IDbConnection

			/// <summary>
			/// Gets or sets the string used to open a database.
			/// </summary>
			/// <returns>
			/// A string containing connection settings.
			/// </returns>
			/// <filterpriority>2</filterpriority>
			public string ConnectionString { get; set; }

			/// <summary>
			/// Gets the time to wait while trying to establish a connection before terminating the attempt and generating an error.
			/// </summary>
			/// <returns>
			/// The time (in seconds) to wait for a connection to open. The default value is 15 seconds.
			/// </returns>
			/// <filterpriority>2</filterpriority>
			public int ConnectionTimeout
			{
				get { return 0; }
			}

			/// <summary>
			/// Gets the name of the current database or the database to be used after a connection is opened.
			/// </summary>
			/// <returns>
			/// The name of the current database or the name of the database to be used once a connection is open. The default value is an empty string.
			/// </returns>
			/// <filterpriority>2</filterpriority>
			public string Database
			{
				get { return string.Empty; }
			}

			/// <summary>
			/// Gets the current state of the connection.
			/// </summary>
			/// <returns>
			/// One of the <see cref="T:System.Data.ConnectionState"/> values.
			/// </returns>
			/// <filterpriority>2</filterpriority>
			public ConnectionState State
			{
				get { return state; }
			}

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			/// <filterpriority>2</filterpriority>
			public void Dispose()
			{
			}

			/// <summary>
			/// Begins a database transaction.
			/// </summary>
			/// <returns>
			/// An object representing the new transaction.
			/// </returns>
			/// <filterpriority>2</filterpriority>
			public IDbTransaction BeginTransaction()
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Begins a database transaction with the specified <see cref="T:System.Data.IsolationLevel"/> value.
			/// </summary>
			/// <returns>
			/// An object representing the new transaction.
			/// </returns>
			/// <param name="il">One of the <see cref="T:System.Data.IsolationLevel"/> values. </param><filterpriority>2</filterpriority>
			public IDbTransaction BeginTransaction(IsolationLevel il)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Closes the connection to the database.
			/// </summary>
			/// <filterpriority>2</filterpriority>
			public void Close()
			{
				state = ConnectionState.Closed;
			}

			/// <summary>
			/// Changes the current database for an open Connection object.
			/// </summary>
			/// <param name="databaseName">The name of the database to use in place of the current database. </param><filterpriority>2</filterpriority>
			public void ChangeDatabase(string databaseName)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Creates and returns a Command object associated with the connection.
			/// </summary>
			/// <returns>
			/// A Command object associated with the connection.
			/// </returns>
			/// <filterpriority>2</filterpriority>
			public IDbCommand CreateCommand()
			{
				return new TestCommand(this);
			}

			/// <summary>
			/// Opens a database connection with the settings specified by the ConnectionString property of the provider-specific Connection object.
			/// </summary>
			/// <filterpriority>2</filterpriority>
			public void Open()
			{
				state = ConnectionState.Open;
			}

			#endregion
		}

		/// <summary>
		/// Тестовая комманда
		/// </summary>
		public class TestCommand : IDbCommand
		{
			/// <summary>
			/// Тестовы конекшин
			/// </summary>
			private readonly TestConnection connection;

			/// <summary>
			/// Initializes a new instance of the <see cref="T:System.Object"/> class.
			/// </summary>
			/// <param name="connection">Тестовое подключение</param>
			public TestCommand(TestConnection connection)
			{
				this.connection = connection;
			}

			#region Implementation of IDbCommand

			/// <summary>
			/// Gets or sets the <see cref="T:System.Data.IDbConnection"/> used by this instance of the <see cref="T:System.Data.IDbCommand"/>.
			/// </summary>
			/// <returns>
			/// The connection to the data source.
			/// </returns>
			/// <filterpriority>2</filterpriority>
			public IDbConnection Connection { get; set; }

			/// <summary>
			/// Gets or sets the transaction within which the Command object of a .NET Framework data provider executes.
			/// </summary>
			/// <returns>
			/// the Command object of a .NET Framework data provider executes. The default value is null.
			/// </returns>
			/// <filterpriority>2</filterpriority>
			public IDbTransaction Transaction { get; set; }

			/// <summary>
			/// Gets or sets the text command to run against the data source.
			/// </summary>
			/// <returns>
			/// The text command to execute. The default value is an empty string ("").
			/// </returns>
			/// <filterpriority>2</filterpriority>
			public string CommandText { get; set; }

			/// <summary>
			/// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
			/// </summary>
			/// <returns>
			/// The time (in seconds) to wait for the command to execute. The default value is 30 seconds.
			/// </returns>
			/// <exception cref="T:System.ArgumentException">The property value assigned is less than 0. </exception><filterpriority>2</filterpriority>
			public int CommandTimeout { get; set; }

			/// <summary>
			/// Indicates or specifies how the <see cref="P:System.Data.IDbCommand.CommandText"/> property is interpreted.
			/// </summary>
			/// <returns>
			/// One of the <see cref="T:System.Data.CommandType"/> values. The default is Text.
			/// </returns>
			/// <filterpriority>2</filterpriority>
			public CommandType CommandType { get; set; }

			/// <summary>
			/// Gets the <see cref="T:System.Data.IDataParameterCollection"/>.
			/// </summary>
			/// <returns>
			/// The parameters of the SQL statement or stored procedure.
			/// </returns>
			/// <filterpriority>2</filterpriority>
			public IDataParameterCollection Parameters
			{
				get { return null; }
			}

			/// <summary>
			/// Gets or sets how command results are applied to the <see cref="T:System.Data.DataRow"/> when used by the <see cref="M:System.Data.IDataAdapter.Update(System.Data.DataSet)"/> method of a <see cref="T:System.Data.Common.DbDataAdapter"/>.
			/// </summary>
			/// <returns>
			/// One of the <see cref="T:System.Data.UpdateRowSource"/> values. The default is Both unless the command is automatically generated. Then the default is None.
			/// </returns>
			/// <exception cref="T:System.ArgumentException">The value entered was not one of the <see cref="T:System.Data.UpdateRowSource"/> values. </exception><filterpriority>2</filterpriority>
			public UpdateRowSource UpdatedRowSource { get; set; }

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			/// <filterpriority>2</filterpriority>
			public void Dispose()
			{
			}

			/// <summary>
			/// Creates a prepared (or compiled) version of the command on the data source.
			/// </summary>
			/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Data.OleDb.OleDbCommand.Connection"/> is not set.-or- The <see cref="P:System.Data.OleDb.OleDbCommand.Connection"/> is not <see cref="M:System.Data.OleDb.OleDbConnection.Open"/>. </exception><filterpriority>2</filterpriority>
			public void Prepare()
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Attempts to cancels the execution of an <see cref="T:System.Data.IDbCommand"/>.
			/// </summary>
			/// <filterpriority>2</filterpriority>
			public void Cancel()
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Creates a new instance of an <see cref="T:System.Data.IDbDataParameter"/> object.
			/// </summary>
			/// <returns>
			/// An IDbDataParameter object.
			/// </returns>
			/// <filterpriority>2</filterpriority>
			public IDbDataParameter CreateParameter()
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
			/// </summary>
			/// <returns>
			/// The number of rows affected.
			/// </returns>
			/// <exception cref="T:System.InvalidOperationException">The connection does not exist.-or- The connection is not open. </exception><filterpriority>2</filterpriority>
			public int ExecuteNonQuery()
			{
				connection.LastCommandText = CommandText;
				return connection.ExecuteNonQueryValue;
			}

			/// <summary>
			/// Executes the <see cref="P:System.Data.IDbCommand.CommandText"/> against the <see cref="P:System.Data.IDbCommand.Connection"/> and builds an <see cref="T:System.Data.IDataReader"/>.
			/// </summary>
			/// <returns>
			/// An <see cref="T:System.Data.IDataReader"/> object.
			/// </returns>
			/// <filterpriority>2</filterpriority>
			public IDataReader ExecuteReader()
			{
				connection.LastCommandText = CommandText;
				throw new NotImplementedException();
			}

			/// <summary>
			/// Executes the <see cref="P:System.Data.IDbCommand.CommandText"/> against the <see cref="P:System.Data.IDbCommand.Connection"/>, and builds an <see cref="T:System.Data.IDataReader"/> using one of the <see cref="T:System.Data.CommandBehavior"/> values.
			/// </summary>
			/// <returns>
			/// An <see cref="T:System.Data.IDataReader"/> object.
			/// </returns>
			/// <param name="behavior">One of the <see cref="T:System.Data.CommandBehavior"/> values. </param><filterpriority>2</filterpriority>
			public IDataReader ExecuteReader(CommandBehavior behavior)
			{
				connection.LastCommandText = CommandText;
				throw new NotImplementedException();
			}

			/// <summary>
			/// Executes the query, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
			/// </summary>
			/// <returns>
			/// The first column of the first row in the resultset.
			/// </returns>
			/// <filterpriority>2</filterpriority>
			public object ExecuteScalar()
			{
				connection.LastCommandText = CommandText;
				return connection.ExecuteScalarValue;
			}

			#endregion
		}
	}
}
