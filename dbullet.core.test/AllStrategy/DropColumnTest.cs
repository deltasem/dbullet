//-----------------------------------------------------------------------
// <copyright file="DropColumnTest.cs" company="delta" author="delta">
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
	/// Тесты DropColumn
	/// </summary>
	[TestFixture]
	public abstract class DropColumnTest : TestBase
	{
		/// <summary>
		/// При пустой колонке должно быть исключение
		/// </summary>
		[Test]
		public void EmptyColumn()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			Assert.Throws<ColumnExpectedException>(() => strategy.DropColumn("test", null));
		}

		/// <summary>
		/// При пустой таблице должно быть исключение
		/// </summary>
		[Test]
		public void EmptyTable()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			Assert.Throws<TableExpectedException>(() => strategy.DropColumn(null, "test"));
		}

		/// <summary>
		/// Удаление столбца
		/// </summary>
		[Test]
		public void RegularDropColumn()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			strategy.DropColumn("testtable", "testcolumn");
			command.VerifySet(x => x.CommandText = "alter table [testtable] drop column [testcolumn]");
		}
	}
}