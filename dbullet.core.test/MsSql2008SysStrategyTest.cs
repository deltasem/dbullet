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
		/// Контекст
		/// </summary>
		public TestContext TestContext { get; set; }

		/// <summary>
		/// Проверка возврата последней версии
		/// </summary>
		[TestMethod]
		public void GetLastVersionTest()
		{
			var connection = new SIDbConnection
			{
				CreateCommand = () => new SIDbCommand
				{
					ExecuteScalar = () => 100500, 
					InstanceBehavior = BehavedBehaviors.DefaultValue
				},
				InstanceBehavior = BehavedBehaviors.DefaultValue
			};
			var target = new MsSql2008SysStrategy(connection, new SIDatabaseStrategy());
			var actual = target.GetLastVersion();
			Assert.AreEqual(100500, actual);
		}

		/// <summary>
		/// Когда еще нет ниодной версии
		/// </summary>
		[TestMethod]
		public void GetLastVersionNull()
		{
			var connection = new SIDbConnection
			{
				CreateCommand = () => new SIDbCommand
				{
					ExecuteScalar = () => System.DBNull.Value,
					InstanceBehavior = BehavedBehaviors.DefaultValue
				},
				InstanceBehavior = BehavedBehaviors.DefaultValue
			};
			var target = new MsSql2008SysStrategy(connection, new SIDatabaseStrategy());
			var actual = target.GetLastVersion();
			Assert.AreEqual(0, actual);
		}

		/// <summary>
		/// Проверка инициализации БД
		/// </summary>
		[TestMethod]
		public void InitDatabaseNewDatabaseTest()
		{
			bool tableWasCreated = false;
			var connection = new SIDbConnection { InstanceBehavior = BehavedBehaviors.DefaultValue };
			var strategy = new SIDatabaseStrategy 
			{ 
				InstanceBehavior = BehavedBehaviors.DefaultValue, 
				IsTableExistString = table => !table.Equals("dbullet"),
				CreateTableTable = table =>
				{
					if (Equals(table.Name, "dbullet"))
					{
						tableWasCreated = true;
					}
				}
			};
			var target = new MsSql2008SysStrategy(connection, strategy);
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
			var connection = new SIDbConnection 
			{ 
				InstanceBehavior = BehavedBehaviors.DefaultValue,
				CreateCommand = () => new SIDbCommand
				{
					ExecuteScalar = () => 1,
					CommandTextSetString = (p) => cmd = p,
					InstanceBehavior = BehavedBehaviors.DefaultValue
				}
			};
			var strategy = new SIDatabaseStrategy
			{
				InstanceBehavior = BehavedBehaviors.DefaultValue
			};

			var target = new MsSql2008SysStrategy(connection, strategy);
			target.SetCurrentVersion(18);
			Assert.AreEqual("insert into dbullet(Version) values(18)", cmd);
		}

		/// <summary>
		/// Тест установки текущей версии
		/// </summary>
		[TestMethod]
		public void RemoveVersionInfo()
		{
			string cmd = string.Empty;
			var connection = new SIDbConnection
			{
				InstanceBehavior = BehavedBehaviors.DefaultValue,
				CreateCommand = () => new SIDbCommand
				{
					ExecuteScalar = () => 1,
					CommandTextSetString = (p) => cmd = p,
					InstanceBehavior = BehavedBehaviors.DefaultValue
				}
			};
			var strategy = new SIDatabaseStrategy
			{
				InstanceBehavior = BehavedBehaviors.DefaultValue
			};

			var target = new MsSql2008SysStrategy(connection, strategy);
			target.RemoveVersionInfo(18);
			Assert.AreEqual("delete from dbullet where version = 18", cmd);
		}
	}
}
