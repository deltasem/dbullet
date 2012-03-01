//-----------------------------------------------------------------------
// <copyright file="InsertRowsTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using dbullet.core.dbs;
using dbullet.core.exception;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// Тесты InsertRows
	/// </summary>
	[TestFixture]
	public abstract class InsertRowsTest : TestBase
	{
		/// <summary>
		/// Добавление в неуказанную таблицу
		/// </summary>
		[Test]
		public void EmptyTable()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(1);
			Assert.Throws<TableExpectedException>(() => strategy.InsertRows(string.Empty, false, new { Test = string.Empty }));
		}

		/// <summary>
		/// Добавление, без указания данных
		/// </summary>
		[Test]
		public void EmptyData()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(1);
			Assert.Throws<ArgumentNullException>(() => strategy.InsertRows("table"));
		}

		/// <summary>
		/// Обычная вставк записи
		/// </summary>
		[Test]
		public void RegularInsert()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(1);
			strategy.InsertRows("testtable", false, new { FIELD_1 = 1, FIELD_2 = "2" });
			command.VerifySet(x => x.CommandText = "insert into [testtable] ([FIELD_1], [FIELD_2]) values('1', '2');");
		}

		/// <summary>
		/// Обычная вставк записи
		/// </summary>
		[Test]
		public void InsertTwoRow()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(1);
			strategy.InsertRows(
				"testtable",
				false, 
				new { FIELD_1 = 1, FIELD_2 = "2" },
				new { FIELD_1 = 3, FIELD_2 = "4" });
			command.VerifySet(x => x.CommandText = "insert into [testtable] ([FIELD_1], [FIELD_2]) values('1', '2');");
			command.VerifySet(x => x.CommandText = "insert into [testtable] ([FIELD_1], [FIELD_2]) values('3', '4');");
		}

		/// <summary>
		/// Если указано identity, то должно использоваться
		/// </summary>
		[Test]
		public void InsertShoudUseIdentity()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(1);
			strategy.InsertRows("testtable", true, new { FIELD_1 = 1, FIELD_2 = "2" });
			command.VerifySet(x => x.CommandText = "set identity_insert [testtable] on; insert into [testtable] ([FIELD_1], [FIELD_2]) values('1', '2'); set identity_insert [testtable] off;");
		}
	}
}