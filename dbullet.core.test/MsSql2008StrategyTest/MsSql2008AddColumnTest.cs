//-----------------------------------------------------------------------
// <copyright file="AddColumnTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbs;
using dbullet.core.engine.MsSql;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// ����� ���������� �������
	/// </summary>
	[TestFixture]
	public class MsSql2008AddColumnTest : AllStrategy.AddColumnTest
	{
		/// <summary>
		/// �������������
		/// </summary>
		[SetUp]
		public void SelfTestInitialize()
		{
			ObjectFactory.Initialize(x => x.For<IDatabaseStrategy>().Use<MsSql2008Strategy>());
			ObjectFactory.Inject(connection.Object);
		}
	}
}