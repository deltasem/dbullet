//-----------------------------------------------------------------------
// <copyright file="DeleteRowsTest.cs" company="delta" created="22.11.2011">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.engine;
using dbullet.core.test.tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// Tests for DeleteRows method
	/// </summary>
	[TestClass]
	public class DeleteRowsTest
	{
		/// <summary>
		/// Fake connection
		/// </summary>
		private TestConnection connection;

		/// <summary>
		/// MSSQL strategy
		/// </summary>
		private MsSql2008Strategy strategy;

		/// <summary>
		/// Initialize test environment
		/// </summary>
		[TestInitialize]
		public void TestInitialize()
		{
			this.connection = new TestConnection();
			this.strategy = new MsSql2008Strategy(this.connection);
		}

		/// <summary>
		/// Should delete all rows if equality conditions is null
		/// </summary>
		[TestMethod]
		public void ShouldDeleteAllRowsIfEqualityConditionsIsNull()
		{
			strategy.DeleteRows("testTable");
			Assert.AreEqual("delete from [testTable]", connection.LastCommandText);
		}

		/// <summary>
		/// Should apply condition
		/// </summary>
		[TestMethod]
		public void ShouldApplyCondition()
		{
			strategy.DeleteRows("testTable", new { ID = 100 });
			Assert.AreEqual("delete from [testTable] where ID = '100'", connection.LastCommandText);
		}

		/// <summary>
		/// Should apply few conditions
		/// </summary>
		[TestMethod]
		public void ShouldApplyFewConditions()
		{
			strategy.DeleteRows("testTable", new { ID1 = 101, ID2 = 102, ID3 = 103 });
			Assert.AreEqual("delete from [testTable] where ID1 = '101' and ID2 = '102' and ID3 = '103'", connection.LastCommandText);
		}

		/// <summary>
		/// Should delete few rows
		/// </summary>
		[TestMethod]
		public void ShouldDeleteFewRows()
		{
			strategy.DeleteRows("testTable", new { ID = 100 }, new { ID = 101 });
			Assert.AreEqual("delete from [testTable] where ID = '100'", connection.AllCommands[0]);
			Assert.AreEqual("delete from [testTable] where ID = '101'", connection.AllCommands[1]);
		}
	}
}