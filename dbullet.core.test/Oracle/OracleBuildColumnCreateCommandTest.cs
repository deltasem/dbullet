//-----------------------------------------------------------------------
// <copyright file="OracleBuildColumnCreateCommandTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Data;
using dbullet.core.dbo;
using dbullet.core.engine.Oracle;
using dbullet.core.test.AllStrategy;
using NUnit.Framework;

namespace dbullet.core.test.Oracle
{
	/// <summary>
	/// Тесты процедуры создания колонки
	/// </summary>
	[TestFixture]
	public class OracleBuildColumnCreateCommandTest : BuildColumnCreateCommandTest
	{
		/// <summary>
		/// Если строка без размера - сгенерить ошибкуx
		/// </summary>
		[Test]
		public override void StringWithoutSize()
		{
			Assert.Throws<ArgumentException>(
				() => OracleTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.String)),
				"String must have length");
		}

		/// <summary>
		/// Обычная колонка-строка
		/// </summary>
		[Test]
		public override void StringDatatype()
		{
			var t = OracleTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.String.Size(50)));
			Assert.AreEqual("\"TESTCOLUMN\" varchar2(50) null", t);
		}

		/// <summary>
		/// Обычная колонка-число
		/// </summary>
		[Test]
		public override void NumericDatatype()
		{
			var t = OracleTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.Decimal.Size(10, 5)));
			Assert.AreEqual("\"TESTCOLUMN\" number(10, 5) null", t);
		}

		/// <summary>
		/// Обычная колонка целое число
		/// </summary>
		[Test]
		public override void IntDataType()
		{
			var t = OracleTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.Int32));
			Assert.AreEqual("\"TESTCOLUMN\" int null", t);
		}

		/// <summary>
		/// Обычная колонка булевый тип
		/// </summary>
		[Test]
		public override void BooleanDataType()
		{
			var t = OracleTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.Boolean));
			Assert.AreEqual("\"TESTCOLUMN\" char(1) null", t);
		}

		/// <summary>
		/// Обычная колонка дата
		/// </summary>
		[Test]
		public override void DateDataType()
		{
			var t = OracleTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.Date));
			Assert.AreEqual("\"TESTCOLUMN\" date null", t);
		}

		/// <summary>
		/// Обычная колонка дата
		/// </summary>
		[Test]
		public override void DateTimeDataType()
		{
			var t = OracleTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.DateTime));
			Assert.AreEqual("\"TESTCOLUMN\" date null", t);
		}

		/// <summary>
		/// Обычная колонка GUID
		/// </summary>
		[Test]
		public override void GuidDataType()
		{
			var t = OracleTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.Guid));
			Assert.AreEqual("\"TESTCOLUMN\" raw(16) null", t);
		}

		/// <summary>
		/// Обычная колонка GUID
		/// </summary>
		[Test]
		public override void XmlDataType()
		{
			var t = OracleTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.Xml));
			Assert.AreEqual("\"TESTCOLUMN\" blob null", t);
		}

		/// <summary>
		/// Binary
		/// </summary>
		[Test]
		public override void BinaryDataTypeWithoutSize()
		{
			Assert.Throws<ArgumentException>(
				() => OracleTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.Binary)), 
				"Binary must have length");
		}

		/// <summary>
		/// Обычная колонка-строка
		/// </summary>
		[Test]
		public override void BinaryDatatype()
		{
			var t = OracleTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", DbType.Binary.Size(50)));
			Assert.AreEqual("\"TESTCOLUMN\" raw(50) null", t);
		}

		/// <summary>
		/// Обычная колонка-строка
		/// </summary>
		[Test]
		public override void VarBinaryDatatype()
		{
			var t = OracleTemplateManager.BuildColumnCreateCommand(new Column("TestColumn", SqlDbType.VarBinary));
			Assert.AreEqual("\"TESTCOLUMN\" blob null", t);
		}
	}
}