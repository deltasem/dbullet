//-----------------------------------------------------------------------
// <copyright file="TestBase.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Data;
using dbullet.core.dbs;
using Moq;
using NUnit.Framework;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// Базовый класс для тестов
	/// </summary>
	[TestFixture]
	public abstract class TestBase
	{
		/// <summary>
		/// Стратегия
		/// </summary>
		protected IDatabaseStrategy strategy { get; set; }

		/// <summary>
		/// Комманда
		/// </summary>
		protected Mock<IDbCommand> command { get; private set; }

		/// <summary>
		/// Соединения
		/// </summary>
		protected Mock<IDbConnection> connection { get; private set; }

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
	}
}