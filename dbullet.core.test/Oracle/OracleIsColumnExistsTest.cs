//-----------------------------------------------------------------------
// <copyright file="OracleIsColumnExistsTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
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
	/// Тесты IsColumnExists
	/// </summary>
	[TestFixture]
	public class OracleIsColumnExistsTest : IsColumnExistsTest
	{
		/// <summary>
		/// Инициализация
		/// </summary>
		[SetUp]
		public void SelfTestInitialize()
		{
			ObjectFactory.Initialize(x => x.For<IDatabaseStrategy>().Use<OracleStrategy>());
			ObjectFactory.Inject(connection.Object);
		}

		/// <summary>
		/// Проверяет, существует ли колонка
		/// </summary>
		protected override string ByNameCommand
		{
			get
			{
				return "select count(*) from user_tab_columns where initcap(table_name) = initcap('ExistingTable') and initcap(column_name) = initcap('ExistingColumn')";
			}
		}
	}
}