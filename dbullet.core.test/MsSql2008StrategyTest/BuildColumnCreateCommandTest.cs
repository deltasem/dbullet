//-----------------------------------------------------------------------
// <copyright file="BuildColumnCreateCommandTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Data;
using dbullet.core.dbo;
using dbullet.core.engine.MsSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// Тесты процедуры создания колонки
	/// </summary>
	[TestClass]
	public class BuildColumnCreateCommandTest
	{
		/// <summary>
		/// Если строка без размера - сгенерить ошибкуx
		/// </summary>
		[TestMethod]
		public void StringWithoutSize()
		{
			AssertHelpers.Throws<ArgumentException>(
				() => MsSqlTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.String)),
				"String must have length");
		}

		/// <summary>
		/// Обычная колонка-строка
		/// </summary>
		[TestMethod]
		public void StringDatatype()
		{
			var t = MsSqlTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.String.Size(50)));
			Assert.AreEqual("TestColumn nvarchar(50) null", t);
		}

		/// <summary>
		/// Обычная колонка-число
		/// </summary>
		[TestMethod]
		public void NumericDatatype()
		{
			var t = MsSqlTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.Decimal.Size(10, 5)));
			Assert.AreEqual("TestColumn decimal(10, 5) null", t);
		}

		/// <summary>
		/// Обычная колонка целое число
		/// </summary>
		[TestMethod]
		public void IntDataType()
		{
			var t = MsSqlTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.Int32));
			Assert.AreEqual("TestColumn int null", t);
		}

		/// <summary>
		/// Обычная колонка булевый тип
		/// </summary>
		[TestMethod]
		public void BooleanDataType()
		{
			var t = MsSqlTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.Boolean));
			Assert.AreEqual("TestColumn bit null", t);
		}

		/// <summary>
		/// Обычная колонка дата
		/// </summary>
		[TestMethod]
		public void DateDataType()
		{
			var t = MsSqlTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.Date));
			Assert.AreEqual("TestColumn date null", t);
		}

		/// <summary>
		/// Обычная колонка дата
		/// </summary>
		[TestMethod]
		public void DateTimeDataType()
		{
			var t = MsSqlTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.DateTime));
			Assert.AreEqual("TestColumn datetime null", t);
		}
	}
}