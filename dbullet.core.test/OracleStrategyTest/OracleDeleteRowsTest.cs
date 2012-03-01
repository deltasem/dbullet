//-----------------------------------------------------------------------
// <copyright file="OracleDeleteRowsTest.cs" company="delta" created="22.11.2011">
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
	/// Tests for DeleteRows method
	/// </summary>
	[TestFixture]
	public class OracleDeleteRowsTest : DeleteRowsTest
	{
		/// <summary>
		/// Initialize test environment
		/// </summary>
		[SetUp]
		public void SelfTestInitialize()
		{
			ObjectFactory.Initialize(x => x.For<IDatabaseStrategy>().Use<OracleStrategy>());
			ObjectFactory.Inject(connection.Object);
		}
	}
}