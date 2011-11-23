//-----------------------------------------------------------------------
// <copyright file="DropTableTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using dbullet.core.engine;
using dbullet.core.exception;
using dbullet.core.test.tools;
using NUnit.Framework;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// Тесты удаления таблиц
	/// </summary>
	[TestFixture]
	public class DropTableTest
	{
		/// <summary>
		/// Удаление таблицы без названия
		/// </summary>
		[Test]
		public void EmptyTable()
		{
			var strategy = new MsSql2008Strategy(new TestConnection());
			AssertHelpers.Throws<TableExpectedException>(() => strategy.DropTable(string.Empty));
		}

		/// <summary>
		/// Удаление таблицы
		/// </summary>
		[Test]
		public void RegularDropTable()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.DropTable("TableForDrop");
			Assert.AreEqual("drop table [TableForDrop]", connection.LastCommandText);
		}
	}
}