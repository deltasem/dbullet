//-----------------------------------------------------------------------
// <copyright file="DropForeignKeyTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using dbullet.core.dbo;
using dbullet.core.engine;
using dbullet.core.test.tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// Тесты удаления внешних ключей
	/// </summary>
	[TestClass]
	public class DropForeignKeyTest
	{
		/// <summary>
		/// Удаление внешнего ключа
		/// </summary>
		[TestMethod]
		public void RegularDropForeignKey()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.DropForeignKey(new ForeignKey("FK_TEST", "TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2", ForeignAction.NoAction));
			Assert.AreEqual("alter table TABLE1 drop constraint FK_TEST", connection.LastCommandText);
		}
	}
}