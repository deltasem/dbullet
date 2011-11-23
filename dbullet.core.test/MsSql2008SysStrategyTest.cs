//-----------------------------------------------------------------------
// <copyright file="MsSql2008SysStrategyTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Data;
using dbullet.core.dbo;
using dbullet.core.dbs;
using dbullet.core.engine;
using Moq;
using NUnit.Framework;

namespace dbullet.core.test
{
	/// <summary>
	/// Тесты для системной стратегии MS SQL 2008
	/// </summary>
	[TestFixture]
	public class MsSql2008SysStrategyTest
	{
		/// <summary>
		/// Заглушка для соединения
		/// </summary>
		private Mock<IDbConnection> dbConnection;

		/// <summary>
		/// Заглушка для системной стратегии
		/// </summary>
		private Mock<IDatabaseStrategy> databaseStrategy;

		/// <summary>
		/// Комманда к базе
		/// </summary>
		private Mock<IDbCommand> createCommand;

		/// <summary>
		/// Тестиреумая стратегия
		/// </summary>
		private MsSql2008SysStrategy target;

		/// <summary>
		/// Инициализация тестов
		/// </summary>
		[SetUp]
		public void TestInitialize()
		{
			createCommand = new Mock<IDbCommand>();
			dbConnection = new Mock<IDbConnection>();
			dbConnection.Setup(x => x.CreateCommand()).Returns(createCommand.Object);
			databaseStrategy = new Mock<IDatabaseStrategy>();
			target = new MsSql2008SysStrategy(dbConnection.Object, databaseStrategy.Object);
		}

		/// <summary>
		/// Проверка возврата последней версии
		/// </summary>
		[Test]
		public void GetLastVersionTest()
		{
			createCommand.Setup(x => x.ExecuteScalar()).Returns(100500);
			var actual = target.GetLastVersion(GetType().Assembly);
			Assert.AreEqual(100500, actual);
		}

		/// <summary>
		/// Последняя версия должна учитывать сборку
		/// </summary>
		[Test]
		public void GetLastVersionShouldUseAssembly()
		{
		  int currentVersion = -1;
			createCommand.SetupSet(x => x.CommandText = It.IsAny<string>())
				.Callback<string>(x => currentVersion = x.Contains(string.Format("'{0}'", GetType().Assembly.GetName().Name)) ? 10 : 0);
			createCommand.Setup(x => x.ExecuteScalar()).Returns(() => currentVersion);
		  var actual = target.GetLastVersion(GetType().Assembly);
		  Assert.AreEqual(10, actual);
		  actual = target.GetLastVersion(typeof(int).Assembly);
		  Assert.AreEqual(0, actual);
		}

		/// <summary>
		/// Когда еще нет ниодной версии
		/// </summary>
		[Test]
		public void GetLastVersionNull()
		{
		  createCommand.Setup(x => x.ExecuteScalar()).Returns(DBNull.Value);
		  var actual = target.GetLastVersion(GetType().Assembly);
		  Assert.AreEqual(0, actual);
		}

		/// <summary>
		/// Проверка инициализации БД
		/// </summary>
		[Test]
		public void InitDatabaseNewDatabaseTest()
		{
			databaseStrategy.Setup(x => x.IsTableExist("dbullet")).Returns(false);
		  target.InitDatabase(GetType().Assembly);
			databaseStrategy.Verify(x => x.CreateTable(It.Is<Table>(y => y.Name == "dbullet")), Times.Once());
		}

		/// <summary>
		/// Тест установки текущей версии
		/// </summary>
		[Test]
		public void SetCurrentVersionTest()
		{
			createCommand.Setup(x => x.ExecuteScalar()).Returns(1);
		  target.SetCurrentVersion(GetType().Assembly, 18);
			createCommand.VerifySet(x => x.CommandText = string.Format("insert into dbullet(Version, Assembly) values(18, '{0}')", GetType().Assembly.GetName().Name), Times.Once());
			createCommand.Verify(x => x.ExecuteScalar(), Times.Once());
		}

		/// <summary>
		/// Тест установки текущей версии
		/// </summary>
		[Test]
		public void RemoveVersionInfo()
		{
			createCommand.Setup(x => x.ExecuteScalar()).Returns(1);
		  
		  target.RemoveVersionInfo(GetType().Assembly, 18);

			createCommand.VerifySet(x => x.CommandText = string.Format("delete from dbullet where version = 18 and Assembly = '{0}'", GetType().Assembly.GetName().Name), Times.Once());
			createCommand.Verify(x => x.ExecuteScalar(), Times.Once());
		}
	}
}
