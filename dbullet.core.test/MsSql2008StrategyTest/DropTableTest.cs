//-----------------------------------------------------------------------
// <copyright file="DropTableTest.cs" company="delta">
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
	/// ����� �������� ������
	/// </summary>
	[TestClass]
	public class DropTableTest
	{
		/// <summary>
		/// �������� ������� ��� ��������
		/// </summary>
		[TestMethod]
		public void EmptyTable()
		{
			var strategy = new MsSql2008Strategy(new TestConnection());
			AssertHelpers.Throws<TableExpectedException>(() => strategy.DropTable(string.Empty));
		}

		/// <summary>
		/// �������� �������
		/// </summary>
		[TestMethod]
		public void RegularDropTable()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.DropTable("TableForDrop");
			Assert.AreEqual("drop table TableForDrop", connection.LastCommandText);
		}
	}
}