//-----------------------------------------------------------------------
// <copyright file="InsertRowsTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using dbullet.core.engine;
using dbullet.core.exception;
using dbullet.core.test.tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// ����� InsertRows
	/// </summary>
	[TestClass]
	public class InsertRowsTest
	{
		/// <summary>
		/// ���������� � ����������� �������
		/// </summary>
		[TestMethod]
		public void EmptyTable()
		{
			var connection = new TestConnection { ExecuteScalarValue = 1 };
			var strategy = new MsSql2008Strategy(connection);
			AssertHelpers.Throws<TableExpectedException>(() => strategy.InsertRows(string.Empty, new { Test = string.Empty }));
		}

		/// <summary>
		/// ����������, ��� �������� ������
		/// </summary>
		[TestMethod]
		public void EmptyData()
		{
			var connection = new TestConnection { ExecuteScalarValue = 1 };
			var strategy = new MsSql2008Strategy(connection);
			AssertHelpers.Throws<ArgumentNullException>(() => strategy.InsertRows("table"));
		}

		/// <summary>
		/// ������� ������ ������
		/// </summary>
		[TestMethod]
		public void RegularInsert()
		{
			var connection = new TestConnection { ExecuteScalarValue = 1 };
			var strategy = new MsSql2008Strategy(connection);
			strategy.InsertRows("testtable", new { FIELD_1 = 1, FIELD_2 = "2" });
			Assert.AreEqual("insert into testtable (FIELD_1, FIELD_2) values('1', '2');", connection.LastCommandText);
		}

		/// <summary>
		/// ������� ������ ������
		/// </summary>
		[TestMethod]
		public void InsertTwoRow()
		{
			var connection = new TestConnection { ExecuteScalarValue = 1 };
			var strategy = new MsSql2008Strategy(connection);
			strategy.InsertRows(
				"testtable",
				new { FIELD_1 = 1, FIELD_2 = "2" },
				new { FIELD_1 = 3, FIELD_2 = "4" });
			Assert.AreEqual("insert into testtable (FIELD_1, FIELD_2) values('1', '2');", connection.AllCommands[0]);
			Assert.AreEqual("insert into testtable (FIELD_1, FIELD_2) values('3', '4');", connection.AllCommands[1]);
		}
	}
}