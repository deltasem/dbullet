//-----------------------------------------------------------------------
// <copyright file="DropIndexTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using dbullet.core.dbo;
using dbullet.core.engine;
using dbullet.core.test.tools;
using NUnit.Framework;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// Тест удаления индексов
	/// </summary>
	[TestFixture]
	public class DropIndexTest
	{
		/// <summary>
		/// Удаление индекса
		/// </summary>
		[Test]
		public void RegularDropIndex()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.DropIndex(new Index("INDEX_NAME", "TABLE_NAME"));
			Assert.AreEqual("drop index INDEX_NAME on [TABLE_NAME]", connection.LastCommandText);
		}		
	}
}