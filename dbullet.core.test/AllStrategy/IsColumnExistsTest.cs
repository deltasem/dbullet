//-----------------------------------------------------------------------
// <copyright file="IsColumnExistsTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using dbullet.core.dbs;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// Тесты MsSql2008IsColumnExists
	/// </summary>	
	[TestFixture]
	public abstract class IsColumnExistsTest : TestBase
	{
		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[Test]
		public void ByName()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(1);
			strategy.IsColumnExists("ExistingTable", "ExistingColumn");
			command.VerifySet(x => x.CommandText = "select count(*) from syscolumns " +
				"where id = object_id(N'ExistingTable') and name = 'ExistingColumn'");
		}

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[Test]
		public void RegularIsColumnExists()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(1);
			var actual = strategy.IsColumnExists("ExistingTable", "ExistingColumn");
			Assert.AreEqual(true, actual);
		}

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[Test]
		public void EmptyTable()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(1);
			Assert.Throws<ArgumentException>(() => strategy.IsColumnExists(string.Empty, string.Empty));
		}

		/// <summary>
		/// Таблица не существует
		/// </summary>
		[Test]
		public void NotExists()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(0);
			var actual = strategy.IsColumnExists("ExistingTable", "ExistingColumn");
			Assert.AreEqual(false, actual);
		}
	}
}