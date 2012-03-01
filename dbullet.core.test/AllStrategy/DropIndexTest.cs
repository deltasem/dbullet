//-----------------------------------------------------------------------
// <copyright file="DropIndexTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbo;
using dbullet.core.dbs;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// Тесты DropIndex
	/// </summary>
	[TestFixture]
	public abstract class DropIndexTest : TestBase
	{
		/// <summary>
		/// Удаление индекса
		/// </summary>
		[Test]
		public void RegularDropIndex()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			strategy.DropIndex(new Index("INDEX_NAME", "TABLE_NAME"));
			command.VerifySet(x => x.CommandText = "drop index INDEX_NAME on [TABLE_NAME]");
		}
	}
}