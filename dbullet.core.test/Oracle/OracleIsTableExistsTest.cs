//-----------------------------------------------------------------------
// <copyright file="OracleIsTableExistsTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbs;
using dbullet.core.engine.Oracle;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.Oracle
{
	/// <summary>
	/// Тесты IsTableExistsTest
	/// </summary>
	[TestFixture]
	public class OracleIsTableExistsTest : AllStrategy.IsTableExistsTest
	{
		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		protected override string ByNameCommandText
		{
			get { return "select count(*) from user_tables where initcap(table_name) = initcap('ExistingTable')"; }
		}

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