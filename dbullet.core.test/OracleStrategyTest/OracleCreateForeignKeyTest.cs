//-----------------------------------------------------------------------
// <copyright file="OracleCreateForeignKeyTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbs;
using dbullet.core.engine.Oracle;
using dbullet.core.test.AllStrategy;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.OracleStrategyTest
{
	/// <summary>
	/// Тесты создания внешних ключей
	/// </summary>
	[TestFixture]
	public class OracleCreateForeignKeyTest : CreateForeignKeyTest
	{
		/// <summary>
		/// Инициализация
		/// </summary>
		[SetUp]
		public void SelfTestInitialize()
		{
			ObjectFactory.Initialize(x => x.For<IDatabaseStrategy>().Use<OracleStrategy>());
			ObjectFactory.Inject(connection.Object);
		}
	}
}