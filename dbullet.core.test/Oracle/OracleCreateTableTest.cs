//-----------------------------------------------------------------------
// <copyright file="OracleCreateTableTest.cs" company="delta">
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
	/// Тесты создания таблиц
	/// </summary>
	[TestFixture]
	public class OracleCreateTableTest : CreateTableTest
	{
		/// <summary>
		/// Нормальное создание
		/// </summary>
		protected override string RegularCreateTableCommand
		{
			get { return "create table \"TestTable\" (\"test\" int null, \"test2\" varchar2(50) null) tablespace \"PRIMARY\";"; }
		}

		/// <summary>
		/// Нормальное создание
		/// </summary>
		protected override string RegularCreateTableWithIdentityCommand
		{
			get { return "create table \"TestTable\" (\"test\" int not null, \"test2\" varchar2(50) null) tablespace \"PRIMARY\"; create sequence TestTable_test minvalue 1 start with 1 increment by 1;"; }
		}

		/// <summary>
		/// Нормальное создание в другой партиции
		/// </summary>
		protected override string CustomPartitionCommand
		{
			get { return "create table \"TestTable\" (\"test\" int null, \"test2\" varchar2(50) null) tablespace \"TESTPARTIOTION\";"; }
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