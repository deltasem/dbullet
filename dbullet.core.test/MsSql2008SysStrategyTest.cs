using System.Data.SqlClient.Moles;
using dbullet.core.dbs;
using dbullet.core.engine;
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
		[HostType("Moles")]
		public void GetLastVersionTest()
		{
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteScalar = p =>
			{
				return 100500;
			};
			var target = new MsSql2008SysStrategy(new MSqlConnection());
			var actual = target.GetLastVersion();
			Assert.AreEqual(100500, actual);
		}

		/// <summary>
		/// Проверка инициализации БД
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void InitDatabaseNewDatabaseTest()
		{
			bool tableWasCreated = false;
			var target = new MsSql2008SysStrategy(new MSqlConnection());
			dbs.Moles.MMsSql2008Strategy.AllInstances.IsTableExistString = (strategy, table) =>
			{
				return !table.Equals("dbullet") && strategy.IsTableExist(table);
			};
			dbs.Moles.MMsSql2008Strategy.AllInstances.CreateTableTable = (strategy, table) =>
			{
				if (Equals(table.Name, "dbullet"))
				{
					tableWasCreated = true;
				}
			};
			target.InitDatabase();
			Assert.IsTrue(tableWasCreated);
		}

		/// <summary>
		/// Тест установки текущей версии
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void SetCurrentVersionTest()
		{
			string cmd = string.Empty;
			MSqlConnection.AllInstances.Open = p => { };
			MSqlConnection.AllInstances.Close = p => { };
			MSqlCommand.AllInstances.ExecuteScalar = p =>
			{
				cmd = p.CommandText;
				return 1;
			};
			var target = new MsSql2008SysStrategy(new MSqlConnection());
			target.SetCurrentVersion(18);
			Assert.AreEqual("insert into dbullet(18)", cmd);
		}
	}
}
