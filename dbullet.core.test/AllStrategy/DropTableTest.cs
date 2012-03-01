//-----------------------------------------------------------------------
// <copyright file="DropTableTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbs;
using dbullet.core.exception;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// тесты DropTable
	/// </summary>
	[TestFixture]
	public abstract class DropTableTest : TestBase
	{
		/// <summary>
		/// Удаление таблицы без названия
		/// </summary>
		[Test]
		public void EmptyTable()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			AssertHelpers.Throws<TableExpectedException>(() => strategy.DropTable(string.Empty));
		}

		/// <summary>
		/// Удаление таблицы
		/// </summary>
		[Test]
		public void RegularDropTable()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			strategy.DropTable("TableForDrop");
			command.VerifySet(x => x.CommandText = "drop table [TableForDrop]");
		}
	}
}