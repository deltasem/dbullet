//-----------------------------------------------------------------------
// <copyright file="IsTableExistsTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using dbullet.core.engine;
using dbullet.core.test.tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// Тесты IsTableExistsTest
	/// </summary>
	[TestClass]
	public class IsTableExistsTest
	{
		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[TestMethod]
		public void ByName()
		{
			var connection = new TestConnection { ExecuteScalarValue = 1 };
			var strategy = new MsSql2008Strategy(connection);
			strategy.IsTableExist("ExistingTable");
			Assert.AreEqual(
				"select count(*) from sysobjects " +
				"where id = object_id(N'ExistingTable') and OBJECTPROPERTY(id, N'IsTable') = 1",
				connection.LastCommandText);
		}

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[TestMethod]
		public void RegularIsTableExists()
		{
			var strategy = new MsSql2008Strategy(new TestConnection { ExecuteScalarValue = 1 });
			var actual = strategy.IsTableExist("ExistingTable");
			Assert.AreEqual(true, actual);
		}

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptyTable()
		{
			var strategy = new MsSql2008Strategy(new TestConnection { ExecuteScalarValue = 1 });
			strategy.IsTableExist(string.Empty);
		}

		/// <summary>
		/// Таблица не существует
		/// </summary>
		[TestMethod]
		public void NotExists()
		{
			var strategy = new MsSql2008Strategy(new TestConnection { ExecuteScalarValue = 0 });
			var actual = strategy.IsTableExist("ExistingTable");
			Assert.AreEqual(false, actual);
		}
	}
}