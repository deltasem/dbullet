using System;
using System.Data.SqlClient.Moles;
using dbullet.core.dbs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test
{
	/// <summary>
	/// Тесты для системной стратегии MS SQL 2008
	/// </summary>
	[TestClass()]
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
			var target = new MsSql2008SysStrategy(new MSqlConnection());
			var actual = target.GetLastVersion();
		}

		/// <summary>
		/// Проверка инициализации БД
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void InitDatabaseTest()
		{
			var target = new MsSql2008SysStrategy(new MSqlConnection());
			target.InitDatabase();
		}

		/// <summary>
		/// Тест установки текущей версии
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void SetCurrentVersionTest()
		{
			var target = new MsSql2008SysStrategy(new MSqlConnection());
			target.SetCurrentVersion();
		}
	}
}
