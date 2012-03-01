//-----------------------------------------------------------------------
// <copyright file="DeleteRowsTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moq;
using NUnit.Framework;
using StructureMap;
using dbullet.core.dbs;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// Тесты DeleteRows
	/// </summary>
	public class DeleteRowsTest : TestBase
	{
		/// <summary>
		/// Should delete all rows if equality conditions is null
		/// </summary>
		[Test]
		public void ShouldDeleteAllRowsIfEqualityConditionsIsNull()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.DeleteRows("testTable");
			command.VerifySet(x => x.CommandText = "delete from [testTable]");
		}

		/// <summary>
		/// Should apply condition
		/// </summary>
		[Test]
		public void ShouldApplyCondition()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.DeleteRows("testTable", new { ID = 100 });
			command.VerifySet(x => x.CommandText = "delete from [testTable] where ID = '100'");
		}

		/// <summary>
		/// Should apply few conditions
		/// </summary>
		[Test]
		public void ShouldApplyFewConditions()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.DeleteRows("testTable", new { ID1 = 101, ID2 = 102, ID3 = 103 });
			command.VerifySet(x => x.CommandText = "delete from [testTable] where ID1 = '101' and ID2 = '102' and ID3 = '103'");
		}

		/// <summary>
		/// Should delete few rows
		/// </summary>
		[Test]
		public void ShouldDeleteFewRows()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.DeleteRows("testTable", new { ID = 100 }, new { ID = 101 });
			command.VerifySet(x => x.CommandText = "delete from [testTable] where ID = '100'");
			command.VerifySet(x => x.CommandText = "delete from [testTable] where ID = '101'");
		}
	}
}