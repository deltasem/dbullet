﻿//-----------------------------------------------------------------------
// <copyright file="TableTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Linq;
using dbullet.core.dbo;
using dbullet.core.exception;
using NUnit.Framework;

namespace dbullet.core.test
{
	/// <summary>
	/// Тесты таблицы
	/// </summary>
	[TestFixture]
	public class TableTest
	{
		/// <summary>
		/// Контектс
		/// </summary>
		public TestContext TestContext { get; set; }

		/// <summary>
		/// Добавление первичного ключа в таблицу без колонки
		/// </summary>
		[Test]
		public void AddPrimaryKeyWithoutColumn()
		{
			Table tbl = new Table("test");
			Assert.Throws<ColumnExpectedException>(() => tbl.AddPrimaryKey("testid"));
		}

		/// <summary>
		/// Добавление первичного ключа должно формировать констраинт в столбце
		/// </summary>
		[Test]
		public void AddPrimaryCollumnConstraint()
		{
			var tbl = new Table("test")
				.AddColumn(new Column("testid", System.Data.DbType.Int32))
				.AddPrimaryKey("testid");
			Assert.IsNotNull(tbl.Columns[0].Constraint);
		}

		/// <summary>
		/// Добавление первичного ключа должно формировать адекватное имя
		/// </summary>
		[Test]
		public void AddPrimaryKeyPrimaryKeyName()
		{
			var tbl = new Table("test")
				.AddColumn(new Column("testid", System.Data.DbType.Int32))
				.AddPrimaryKey("testid");
			Assert.AreEqual("PK_TEST", tbl.Columns[0].Constraint.Name);
		}

		/// <summary>
		/// Добавление дефалта без колонки
		/// </summary>
		[Test]
		public void AddDefaultWithoutColumn()
		{
			var tbl = new Table("test");
			Assert.Throws<ColumnExpectedException>(() => tbl.Default("test"));
		}

		/// <summary>
		/// Добавление дефалта для колонки с первичным ключем
		/// </summary>
		[Test]
		public void AddDefaultAtPrimaryKeyColumn()
		{
			var tbl = new Table("test")
				.AddColumn(new Column("testid", System.Data.DbType.Int32))
				.AddPrimaryKey("testid");
			Assert.Throws<ConflictingDataException>(() => tbl.Default("test"));
		}

		/// <summary>
		/// Добавление дефалта в последнюю колонку
		/// </summary>
		[Test]
		public void AddOneDefaultAtLastColumn()
		{
			var tbl = new Table("test")
				.AddColumn(new Column("testid", System.Data.DbType.Int32))
				.AddColumn(new Column("col2", System.Data.DbType.Int32))
				.AddColumn(new Column("col3", System.Data.DbType.Int32))
				.AddPrimaryKey("testid");
			tbl.Default("100500");
			var constraint = tbl.Columns.Last().Constraint as ValueDefault;
			Assert.IsNotNull(constraint);
			Assert.AreEqual("100500", constraint.Value);
		}

		/// <summary>
		/// Добавление дефалта должно генерировать его имя
		/// </summary>
		[Test]
		public void AddDefaultAutoName()
		{
			var tbl = new Table("test")
				.AddColumn(new Column("testid", System.Data.DbType.Int32));
			tbl.Default("100500");
			Assert.AreEqual("DF_TEST_TESTID", tbl.Columns[0].Constraint.Name);
		}
	}
}