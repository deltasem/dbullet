//-----------------------------------------------------------------------
// <copyright file="MsSql2008SysStrategyTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Data.Moles;
using dbullet.core.dbs.Moles;
using dbullet.core.engine;
using Microsoft.Moles.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test
{
	/// <summary>
	/// Тесты для системной стратегии MS SQL 2008
	/// </summary>
	[TestClass]
	public class MsSql2008SysStrategyTest
	{
		/// <summary>
		/// Заглушка для соединения
		/// </summary>
		private SIDbConnection dbConnection;

		/// <summary>
		/// Заглушка для системной стратегии
		/// </summary>
		private SIDatabaseStrategy databaseStrategy;

		/// <summary>
		/// Комманда к базе
		/// </summary>
		private SIDbCommand createCommand;

		/// <summary>
		/// Тестиреумая стратегия
		/// </summary>
		private MsSql2008SysStrategy target;

		/// <summary>
		/// Инициализация тестов
		/// </summary>
		[TestInitialize]
		public void TestInitialize()
		{
			createCommand = new SIDbCommand { InstanceBehavior = BehavedBehaviors.DefaultValue };
			dbConnection = new SIDbConnection
			               	{
												CreateCommand = () => createCommand,
			               		InstanceBehavior = BehavedBehaviors.DefaultValue
			               	};
			databaseStrategy = new SIDatabaseStrategy { InstanceBehavior = BehavedBehaviors.DefaultValue };
			target = new MsSql2008SysStrategy(dbConnection, databaseStrategy);
		}

		/// <summary>
		/// Проверка возврата последней версии
		/// </summary>
		[TestMethod]
		public void GetLastVersionTest()
		{
			createCommand.ExecuteScalar = () => 100500;
			var actual = target.GetLastVersion(GetType().Assembly);
			Assert.AreEqual(100500, actual);
		}

		/// <summary>
		/// Последняя версия должна учитывать сборку
		/// </summary>
		[TestMethod]
		public void GetLastVersionShouldUseAssembly()
		{
			int currentVersion = -1;
			createCommand.CommandTextSetString = x => { currentVersion = x.Contains(string.Format("'{0}'", GetType().Assembly.GetName().Name)) ? 10 : 0; };
			createCommand.ExecuteScalar = () => currentVersion;
			var actual = target.GetLastVersion(GetType().Assembly);
			Assert.AreEqual(10, actual);
			actual = target.GetLastVersion(typeof(int).Assembly);
			Assert.AreEqual(0, actual);
		}

		/// <summary>
		/// Когда еще нет ниодной версии
		/// </summary>
		[TestMethod]
		public void GetLastVersionNull()
		{
			createCommand.ExecuteScalar = () => System.DBNull.Value;
			var actual = target.GetLastVersion(GetType().Assembly);
			Assert.AreEqual(0, actual);
		}

		/// <summary>
		/// Проверка инициализации БД
		/// </summary>
		[TestMethod]
		public void InitDatabaseNewDatabaseTest()
		{
			bool tableWasCreated = false;
			databaseStrategy.IsTableExistString = table => !table.Equals("dbullet");
			databaseStrategy.CreateTableTable = table => { tableWasCreated = Equals(table.Name, "dbullet"); };
			target.InitDatabase();
			Assert.IsTrue(tableWasCreated);
		}

		/// <summary>
		/// Тест установки текущей версии
		/// </summary>
		[TestMethod]
		public void SetCurrentVersionTest()
		{
			string cmd = string.Empty;
			createCommand.ExecuteScalar = () => 1;
			createCommand.CommandTextSetString = p => cmd = p;
			target.SetCurrentVersion(GetType().Assembly, 18);
			Assert.AreEqual(string.Format("insert into dbullet(Version, Assembly) values(18, '{0}')", GetType().Assembly.GetName().Name), cmd);
		}

		/// <summary>
		/// Тест установки текущей версии
		/// </summary>
		[TestMethod]
		public void RemoveVersionInfo()
		{
			string cmd = string.Empty;
			createCommand.ExecuteScalar = () => 1;
			createCommand.CommandTextSetString = (p) => cmd = p;
			target.RemoveVersionInfo(GetType().Assembly, 18);
			Assert.AreEqual(string.Format("delete from dbullet where version = 18 and Assembly = '{0}'", GetType().Assembly.GetName().Name), cmd);
		}
	}
}
