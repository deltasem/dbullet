//-----------------------------------------------------------------------
// <copyright file="MsSql2008DeleteRowsTest.cs" company="delta" created="22.11.2011">
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
	/// Tests for DeleteRows method
	/// </summary>
	[TestFixture]
	public class MsSql2008DeleteRowsTest : DeleteRowsTest
	{
		/// <summary>
		/// Комманда уделения всех записей
		/// </summary>
		protected override string ShouldDeleteAllRowsIfEqualityConditionsIsNullCommand
		{
			get { return "delete from [testTable]"; }
		}

		/// <summary>
		/// Комманда удаления записи по условию
		/// </summary>
		protected override string ShouldApplyConditionCommand
		{
			get { return "delete from [testTable] where ID = '100'"; }
		}

		/// <summary>
		/// Комманда удаления записей по нескольким условиям
		/// </summary>
		protected override string ShouldApplyFewConditionsCommand
		{
			get { return "delete from [testTable] where ID1 = '101' and ID2 = '102' and ID3 = '103'"; }
		}

		/// <summary>
		/// Комманда удаления записи по условию
		/// </summary>
		protected override string ShouldDeleteFewRowsCommand
		{
			get { return "delete from [testTable] where ID = '101'"; }
		}

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