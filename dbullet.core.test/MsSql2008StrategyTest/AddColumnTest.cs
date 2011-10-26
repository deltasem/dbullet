//-----------------------------------------------------------------------
// <copyright file="AddColumnTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Data;
using dbullet.core.dbo;
using dbullet.core.engine;
using dbullet.core.test.tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// Тесты добавления колонки
	/// </summary>
	[TestClass]
	public class AddColumnTest
	{
		/// <summary>
		/// Нельзя добавить колонку без дефалта, и не нулл
		/// </summary>
		[TestMethod]
		public void NotNullNotDefaul()
		{
			var strategy = new MsSql2008Strategy(null);
			AssertHelpers.Throws<ArgumentException>(() => strategy.AddColumn(new Table("TestTable"), new Column("TestColumn", DbType.Int32, false)));
		}

		/// <summary>
		/// Добавление столбца с allow null
		/// </summary>
		[TestMethod]
		public void AddWithNull()
		{
			var con = new TestConnection();
			var strategy = new MsSql2008Strategy(con);
			strategy.AddColumn(new Table("TestTable"), new Column("TestColumn", DbType.Int32));
			Assert.AreEqual("alter table [TestTable] add [TestColumn] int null", con.LastCommandText);
		}

		/// <summary>
		/// Добавление столбца с дефалтом
		/// </summary>
		[TestMethod]
		public void AddWithValueDefault()
		{
			var con = new TestConnection();
			var strategy = new MsSql2008Strategy(con);
			strategy.AddColumn(new Table("TestTable"), new Column("TestColumn", DbType.Int32, false) { Constraint = new ValueDefault("df_test", "100500") });
			Assert.AreEqual("alter table [TestTable] add [TestColumn] int not null constraint df_test default '100500'", con.LastCommandText);
		}

		/// <summary>
		/// Добавление столбца с дефалтом - время
		/// </summary>
		[TestMethod]
		public void AddWithDateDefault()
		{
			var con = new TestConnection();
			var strategy = new MsSql2008Strategy(con);
			strategy.AddColumn(new Table("TestTable"), new Column("TestColumn", DbType.Int32, false) { Constraint = new StandartDefault("df_test", StandartDefaultType.date) });
			Assert.AreEqual("alter table [TestTable] add [TestColumn] int not null constraint df_test default 'getdate()'", con.LastCommandText);
		}

		/// <summary>
		/// Добавление столбца с дефалтом - GUID
		/// </summary>
		[TestMethod]
		public void AddWithGuidDefault()
		{
			var con = new TestConnection();
			var strategy = new MsSql2008Strategy(con);
			strategy.AddColumn(new Table("TestTable"), new Column("TestColumn", DbType.Int32, false) { Constraint = new StandartDefault("df_test", StandartDefaultType.guid) });
			Assert.AreEqual("alter table [TestTable] add [TestColumn] int not null constraint df_test default 'newid()'", con.LastCommandText);
		}	
	}
}