//-----------------------------------------------------------------------
// <copyright file="TableTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using dbullet.core.dbo;
using dbullet.core.exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test
{
	/// <summary>
	/// Тесты таблицы
	/// </summary>
	[TestClass]
	public class TableTest
	{
		/// <summary>
		/// Контектс
		/// </summary>
		public TestContext TestContext { get; set; }

		/// <summary>
		/// Добавление первичного ключа в таблицу без колонки
		/// </summary>
		[TestMethod]
		public void AddPrimaryKeyWithoutColumn()
		{
			Table tbl = new Table("test");
			AssertHelpers.Throws<CollumnExpectedException>(() => tbl.AddPrimaryKey("testid"));
		}

		/// <summary>
		/// Добавление первичного ключа должно формировать констраинт в столбце
		/// </summary>
		[TestMethod]
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
		[TestMethod]
		public void AddPrimaryKeyPrimaryKeyName()
		{
			var tbl = new Table("test")
				.AddColumn(new Column("testid", System.Data.DbType.Int32))
				.AddPrimaryKey("testid");
			Assert.AreEqual("PK_TEST", tbl.Columns[0].Constraint.Name);
		}
	}
}