//-----------------------------------------------------------------------
// <copyright file="OracleSysStrategyTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbs;
using dbullet.core.engine.Oracle;
using dbullet.core.test.AllStrategy;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.Oracle
{
	/// <summary>
	/// Тесты для системной стратегии oracle
	/// </summary>
	[TestFixture]
	public class OracleSysStrategyTest : SysStrategyTest
	{
		/// <summary>
		/// Инициализация тестов
		/// </summary>
		[SetUp]
		public void SelfTestInitialize()
		{
			ObjectFactory.Initialize(x =>
			{
				x.For<IDatabaseStrategy>().Use<OracleStrategy>();
				x.For<ISysDatabaseStrategy>().Use<OracleSysStrategy>();
			});

			ObjectFactory.Inject(connection.Object);
		}
	}
}
