//-----------------------------------------------------------------------
// <copyright file="DropIndexTest.cs" company="delta">
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
	/// ���� �������� ��������
	/// </summary>
	[TestClass]
	public class DropIndexTest
	{
		/// <summary>
		/// �������� �������
		/// </summary>
		[TestMethod]
		public void RegularDropIndex()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.DropIndex(new Index("INDEX_NAME", "TABLE_NAME"));
			Assert.AreEqual("drop index INDEX_NAME on TABLE_NAME", connection.LastCommandText);
		}		
	}
}