//-----------------------------------------------------------------------
// <copyright file="OracleAddColumnTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbs;
using dbullet.core.engine.Oracle;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.Oracle
{
	/// <summary>
	/// Тесты добавления колонки
	/// </summary>
	[TestFixture]
	public class OracleAddColumnTest : AllStrategy.AddColumnTest
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