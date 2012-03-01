//-----------------------------------------------------------------------
// <copyright file="MsSql2008DropColumnTest.cs" company="delta" created="24.07.2011">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbs;
using dbullet.core.engine.MsSql;
using dbullet.core.test.AllStrategy;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// Тесты удаления столбцов
	/// </summary>
	[TestFixture]
	public class MsSql2008DropColumnTest : DropColumnTest
	{
		/// <summary>
		/// Инициализация
		/// </summary>
		[SetUp]
		public void SelfTestInitialize()
		{
			ObjectFactory.Initialize(x => x.For<IDatabaseStrategy>().Use<MsSql2008Strategy>());
			ObjectFactory.Inject(connection.Object);
		}
	}
}