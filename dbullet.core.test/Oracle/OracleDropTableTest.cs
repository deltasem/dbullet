//-----------------------------------------------------------------------
// <copyright file="OracleDropTableTest.cs" company="delta" author="delta">
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
	/// Тесты удаления таблиц
	/// </summary>
	[TestFixture]
	public class OracleDropTableTest : DropTableTest
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
		/// Удаление таблицы
		/// </summary>
		protected override string RegularDropTableCommand
		{
			get { return "declare tmp int;" +
			             " begin" +
			             " execute immediate" +
			             " 'drop table \"TABLEFORDROP\"';" +
									 " select count(*) into tmp from user_sequences where initcap(sequence_name) = initcap('TABLEFORDROP_ID');" +
			             " if tmp > 0 then" +
									 " execute immediate 'drop sequence \"TABLEFORDROP_ID\"';" +
			             " end if;" +
			             " end;";
			}
		}
	}
}