//-----------------------------------------------------------------------
// <copyright file="IsColumnExistsTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using dbullet.core.engine;
using dbullet.core.engine.MsSql;
using dbullet.core.test.tools;
using NUnit.Framework;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// Тесты IsColumnExistsTest
	/// </summary>
	[TestFixture]
	public class IsColumnExistsTest
	{
		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[Test]
		public void ByName()
		{
			var connection = new TestConnection { ExecuteScalarValue = 1 };
			var strategy = new MsSql2008Strategy(connection);
			strategy.IsColumnExists("ExistingTable", "ExistingColumn");
			Assert.AreEqual(
				"select count(*) from syscolumns " +
				"where id = object_id(N'ExistingTable') and name = 'ExistingColumn'",
				connection.LastCommandText);
		}

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[Test]
		public void RegularIsColumnExists()
		{
			var strategy = new MsSql2008Strategy(new TestConnection { ExecuteScalarValue = 1 });
			var actual = strategy.IsColumnExists("ExistingTable", "ExistingColumn");
			Assert.AreEqual(true, actual);
		}

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptyTable()
		{
			var strategy = new MsSql2008Strategy(new TestConnection { ExecuteScalarValue = 1 });
			strategy.IsColumnExists(string.Empty, string.Empty);
		}

		/// <summary>
		/// Таблица не существует
		/// </summary>
		[Test]
		public void NotExists()
		{
			var strategy = new MsSql2008Strategy(new TestConnection { ExecuteScalarValue = 0 });
			var actual = strategy.IsColumnExists("ExistingTable", "ExistingColumn");
			Assert.AreEqual(false, actual);
		}
	}
}