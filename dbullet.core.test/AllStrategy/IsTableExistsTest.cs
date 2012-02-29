//-----------------------------------------------------------------------
// <copyright file="IsTableExistsTest.cs" company="SofTrust" author="SKiryshin">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using dbullet.core.dbs;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// Тесты IsTableExists
	/// </summary>
	[TestFixture]
	public abstract class IsTableExistsTest : TestBase
	{
		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		protected abstract string ByNameCommandText { get; }

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[Test]
		public void ByName()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(1);
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.IsTableExist("ExistingTable");
			command.VerifySet(x => x.CommandText = ByNameCommandText);
		}

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[Test]
		public void RegularIsTableExists()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(1);
			var actual = strategy.IsTableExist("ExistingTable");
			Assert.AreEqual(true, actual);
		}

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptyTable()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(1);
			strategy.IsTableExist(string.Empty);
		}

		/// <summary>
		/// Таблица не существует
		/// </summary>
		[Test]
		public void NotExists()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(0);
			var actual = strategy.IsTableExist("ExistingTable");
			Assert.AreEqual(false, actual);
		}
	}
}