//-----------------------------------------------------------------------
// <copyright file="OracleAddColumnTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbs;
using dbullet.core.engine.Oracle;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.Oracle
{
	/// <summary>
	/// Тесты добавления колонки
	/// </summary>
	[TestFixture]
	public class OracleAddColumnTest : AllStrategy.AddColumnTest
	{
		/// <summary>
		/// Добавление столбца с allow null
		/// </summary>
		protected override string AddWithNullCommand
		{
			get { return "begin execute immediate 'alter table \"TESTTABLE\" add \"TESTCOLUMN\" int null'; end;"; }
		}

		/// <summary>
		/// Добавление столбца с дефалтом
		/// </summary>
		protected override string AddWithValueDefaultCommand
		{
			get { return "begin execute immediate 'alter table \"TESTTABLE\" add \"TESTCOLUMN\" int default ''100500'' not null'; end;"; }
		}

		/// <summary>
		/// Добавление столбца с дефалтом - время
		/// </summary>
		protected override string AddWithDateDefaultCommand
		{
			get { return "begin execute immediate 'alter table \"TESTTABLE\" add \"TESTCOLUMN\" int default sysdate not null'; end;"; }
		}

		/// <summary>
		/// Добавление столбца с дефалтом - GUID
		/// </summary>
		protected override string AddWithGuidDefaultCommand
		{
			get { return "begin execute immediate 'alter table \"TESTTABLE\" add \"TESTCOLUMN\" int default sys_guid() not null'; end;"; }
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