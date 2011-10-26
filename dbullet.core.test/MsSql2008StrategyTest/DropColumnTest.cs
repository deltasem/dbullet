//-----------------------------------------------------------------------
// <copyright file="DropColumnTest.cs" company="delta" created="24.07.2011">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.engine;
using dbullet.core.exception;
using dbullet.core.test.tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// Тесты удаления столбцов
	/// </summary>
	[TestClass]
	public class DropColumnTest
	{
		/// <summary>
		/// При пустой колонке должно быть исключение
		/// </summary>
		[TestMethod]
		public void EmptyColumn()
		{
			var strategy = new MsSql2008Strategy(new TestConnection());
			AssertHelpers.Throws<ColumnExpectedException>(() => strategy.DropColumn("test", null));
		}

		/// <summary>
		/// При пустой таблице должно быть исключение
		/// </summary>
		[TestMethod]
		public void EmptyTable()
		{
			var strategy = new MsSql2008Strategy(new TestConnection());
			AssertHelpers.Throws<TableExpectedException>(() => strategy.DropColumn(null, "test"));
		}

		/// <summary>
		/// Удаление столбца
		/// </summary>
		[TestMethod]
		public void RegularDropColumn()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.DropColumn("testtable", "testcolumn");
			Assert.AreEqual("alter table [testtable] drop column [testcolumn]", connection.LastCommandText);
		}
	}
}