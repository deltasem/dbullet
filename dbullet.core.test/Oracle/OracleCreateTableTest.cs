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
			get { return "begin execute immediate 'create table \"TestTable\" (\"test\" int null, \"test2\" varchar2(50) null)'; end;"; }
		}

		/// <summary>
		/// Нормальное создание
		/// </summary>
		protected override string RegularCreateTableWithIdentityCommand
		{
			get { return "begin execute immediate 'create table \"TestTable\" (\"test\" int not null, \"test2\" varchar2(50) null)'; execute immediate 'create sequence TestTable_test minvalue 1 start with 1 increment by 1'; end;"; }
		}

		/// <summary>
		/// Нормальное создание в другой партиции
		/// </summary>
		protected override string CustomPartitionCommand
		{
			get { return "begin execute immediate 'create table \"TestTable\" (\"test\" int null, \"test2\" varchar2(50) null) tablespace \"TESTPARTIOTION\"'; end;"; }
		}

		/// <summary>
		/// Нормальное создание в другой партиции c первичным ключем
		/// </summary>
		protected override string CustomPartitionWithPrimaryKeyCommand
		{
			get 
			{
				return "begin execute immediate 'create table \"TestTable\" (\"testid\" int null, \"test2\" varchar2(50) null) tablespace \"TESTPARTIOTION\"';" +
					" execute immediate 'alter table \"TestTable\" add constraint \"PK_TESTTABLE\" primary key (\"testid\") using index'; end;";
			}
		}

		/// <summary>
		/// Создание таблицы с первичным ключем в не стандартной партиции
		/// </summary>
		protected override string WithPrimaryKeyCustomPartitionCommand
		{
			get
			{
				return "begin execute immediate 'create table \"TestTable\" (\"testid\" int null, \"test2\" varchar2(50) null)';" +
					" execute immediate 'alter table \"TestTable\" add constraint \"PK_TESTTABLE\" primary key (\"testid\") using index tablespace \"TESTPARTIOTION\"'; end;";
			}
		}

		/// <summary>
		/// Создание с дефалтом
		/// </summary>
		protected override string WithDefaultCommand
		{
			get { return "begin execute immediate 'create table \"TestTable\" (\"test\" int default ''100500'' null, \"test2\" varchar2(50) default ''this is the test'' null)'; end;"; }
		}

		/// <summary>
		/// Со стандартным дефалтом - системное время
		/// </summary>
		protected override string WithStandartDefaultDateCommand
		{
			get { return "begin execute immediate 'create table \"TestTable\" (\"test\" int default sysdate null)'; end;"; }
		}

		/// <summary>
		/// Со стандартным дефалтом - новый GUID
		/// </summary>
		protected override string WithStandartDefaultGuidCommand
		{
			get { return "begin execute immediate 'create table \"TestTable\" (\"test\" int default sys_guid() null)'; end;"; }
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