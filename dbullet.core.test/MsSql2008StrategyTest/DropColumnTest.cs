//-----------------------------------------------------------------------
// <copyright file="DropColumnTest.cs" company="delta" created="24.07.2011">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.engine;
using dbullet.core.engine.MsSql;
using dbullet.core.exception;
using dbullet.core.test.tools;
using NUnit.Framework;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// Тесты удаления столбцов
	/// </summary>
	[TestFixture]
	public class DropColumnTest
	{
		/// <summary>
		/// При пустой колонке должно быть исключение
		/// </summary>
		[Test]
		public void EmptyColumn()
		{
			var strategy = new MsSql2008Strategy(new TestConnection());
			AssertHelpers.Throws<ColumnExpectedException>(() => strategy.DropColumn("test", null));
		}

		/// <summary>
		/// При пустой таблице должно быть исключение
		/// </summary>
		[Test]
		public void EmptyTable()
		{
			var strategy = new MsSql2008Strategy(new TestConnection());
			AssertHelpers.Throws<TableExpectedException>(() => strategy.DropColumn(null, "test"));
		}

		/// <summary>
		/// Удаление столбца
		/// </summary>
		[Test]
		public void RegularDropColumn()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.DropColumn("testtable", "testcolumn");
			Assert.AreEqual("alter table [testtable] drop column [testcolumn]", connection.LastCommandText);
		}
	}
}