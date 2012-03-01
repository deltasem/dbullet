//-----------------------------------------------------------------------
// <copyright file="MsSql2008CreateTableTest.cs" company="delta">
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
	/// Тесты создания таблиц
	/// </summary>
	[TestFixture]
	public class MsSql2008CreateTableTest : CreateTableTest
	{
		/// <summary>
		/// Нормальное создание
		/// </summary>
		protected override string RegularCreateTableCommand
		{
			get { return "create table [TestTable] ([test] int null, [test2] nvarchar(50) null) on [PRIMARY]"; }
		}

		/// <summary>
		/// Нормальное создание
		/// </summary>
		protected override string RegularCreateTableWithIdentityCommand
		{
			get { return "create table [TestTable] ([test] int not null identity(1, 1), [test2] nvarchar(50) null) on [PRIMARY]"; }
		}

		/// <summary>
		/// Инициализация
		/// </summary>
		[SetUp]
		public void SelfTestInitialize()
		{
			ObjectFactory.Initialize(x => x.For<IDatabaseStrategy>().Use<MsSql2008Strategy>());
			ObjectFactory.Inject(connection.Object);
		}
	}
}