﻿//-----------------------------------------------------------------------
// <copyright file="MsSql2008DeleteRowsTest.cs" company="delta" created="22.11.2011">
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
	/// Tests for DeleteRows method
	/// </summary>
	[TestFixture]
	public class MsSql2008DeleteRowsTest : DeleteRowsTest
	{
		/// <summary>
		/// Initialize test environment
		/// </summary>
		[SetUp]
		public void SelfTestInitialize()
		{
			ObjectFactory.Initialize(x => x.For<IDatabaseStrategy>().Use<MsSql2008Strategy>());
			ObjectFactory.Inject(connection.Object);
		}
	}
}