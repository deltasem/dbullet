//-----------------------------------------------------------------------
// <copyright file="DeleteRowsTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbs;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// Тесты DeleteRows
	/// </summary>
	[TestFixture]
	public abstract class DeleteRowsTest : TestBase
	{
		/// <summary>
		/// Комманда уделения всех записей
		/// </summary>
		protected abstract string ShouldDeleteAllRowsIfEqualityConditionsIsNullCommand { get; }

		/// <summary>
		/// Комманда удаления записи по условию
		/// </summary>
		protected abstract string ShouldApplyConditionCommand { get; }

		/// <summary>
		/// Комманда удаления записей по нескольким условиям
		/// </summary>
		protected abstract string ShouldApplyFewConditionsCommand { get; }

		/// <summary>
		/// Комманда удаления записи по условию
		/// </summary>
		protected abstract string ShouldDeleteFewRowsCommand { get; }

		/// <summary>
		/// Should delete all rows if equality conditions is null
		/// </summary>
		[Test]
		public void ShouldDeleteAllRowsIfEqualityConditionsIsNull()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.DeleteRows("testTable");
			command.VerifySet(x => x.CommandText = ShouldDeleteAllRowsIfEqualityConditionsIsNullCommand);
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
			command.VerifySet(x => x.CommandText = ShouldApplyConditionCommand);
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
			command.VerifySet(x => x.CommandText = ShouldApplyFewConditionsCommand);
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
			command.VerifySet(x => x.CommandText = ShouldApplyConditionCommand);
			command.VerifySet(x => x.CommandText = ShouldDeleteFewRowsCommand);
		}
	}
}