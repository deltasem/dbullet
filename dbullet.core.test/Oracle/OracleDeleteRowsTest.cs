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

namespace dbullet.core.test.Oracle
{
	/// <summary>
	/// Tests for DeleteRows method
	/// </summary>
	[TestFixture]
	public class OracleDeleteRowsTest : DeleteRowsTest
	{
		/// <summary>
		/// Комманда уделения всех записей
		/// </summary>
		protected override string ShouldDeleteAllRowsIfEqualityConditionsIsNullCommand
		{
			get { return "delete from \"TESTTABLE\""; }
		}

		/// <summary>
		/// Комманда удаления записи по условию
		/// </summary>
		protected override string ShouldApplyConditionCommand
		{
			get { return "delete from \"TESTTABLE\" where \"ID\" = '100'"; }
		}

		/// <summary>
		/// Комманда удаления записей по нескольким условиям
		/// </summary>
		protected override string ShouldApplyFewConditionsCommand
		{
			get { return "delete from \"TESTTABLE\" where \"ID1\" = '101' and \"ID2\" = '102' and \"ID3\" = '103'"; }
		}

		/// <summary>
		/// Комманда удаления записи по условию
		/// </summary>
		protected override string ShouldDeleteFewRowsCommand
		{
			get { return "delete from \"TESTTABLE\" where \"ID\" = '101'"; }
		}

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