//-----------------------------------------------------------------------
// <copyright file="IsTableExistsTest.cs" company="SofTrust" author="SKiryshin">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Data;
using dbullet.core.dbs;
using dbullet.core.engine.MsSql;
using dbullet.core.test.tools;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// Тесты IsTableExists
	/// </summary>
	[TestFixture]
	public abstract class IsTableExistsTest
	{
		/// <summary>
		/// Стратегия
		/// </summary>
		protected IDatabaseStrategy strategy { get; private set; }

		/// <summary>
		/// Комманда
		/// </summary>
		protected Mock<IDbCommand> command { get; private set; }

		/// <summary>
		/// Соединения
		/// </summary>
		protected Mock<IDbConnection> connection { get; private set; }

		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		protected abstract string ByNameCommandText { get; }

		/// <summary>
		/// Инициализация
		/// </summary>
		[SetUp]
		public void TestInitialize()
		{
			connection = new Mock<IDbConnection>();
			command = new Mock<IDbCommand>();
			connection.Setup(x => x.CreateCommand()).Returns(command.Object);
		}

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
			var strategy = new MsSql2008Strategy(new TestConnection { ExecuteScalarValue = 1 });
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
			var strategy = new MsSql2008Strategy(new TestConnection { ExecuteScalarValue = 1 });
			strategy.IsTableExist(string.Empty);
		}

		/// <summary>
		/// Таблица не существует
		/// </summary>
		[Test]
		public void NotExists()
		{
			var strategy = new MsSql2008Strategy(new TestConnection { ExecuteScalarValue = 0 });
			var actual = strategy.IsTableExist("ExistingTable");
			Assert.AreEqual(false, actual);
		}
	}
}