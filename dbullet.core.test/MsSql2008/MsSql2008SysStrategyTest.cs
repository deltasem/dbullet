//-----------------------------------------------------------------------
// <copyright file="MsSql2008SysStrategyTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbs;
using dbullet.core.engine.MsSql;
using dbullet.core.test.AllStrategy;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.MsSql2008
{
	/// <summary>
	/// Тесты для системной стратегии MS SQL 2008
	/// </summary>
	[TestFixture]
	public class MsSql2008SysStrategyTest : SysStrategyTest
	{
		/// <summary>
		/// Инициализация тестов
		/// </summary>
		[SetUp]
		public void SelfTestInitialize()
		{
			ObjectFactory.Initialize(x =>
			                         	{
																	x.For<IDatabaseStrategy>().Use<MsSql2008Strategy>();
																	x.For<ISysDatabaseStrategy>().Use<MsSql2008SysStrategy>();
			                         	});
			
			ObjectFactory.Inject(connection.Object);
		}
	}
}
