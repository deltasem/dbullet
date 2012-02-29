//-----------------------------------------------------------------------
// <copyright file="AddColumnTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Data;
using dbullet.core.dbo;
using dbullet.core.dbs;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// Тесты AddColumn
	/// </summary>
	[TestFixture]
	public abstract class AddColumnTest : TestBase
	{
		/// <summary>
		/// Нельзя добавить колонку без дефалта, и не нулл
		/// </summary>
		[Test]
		public void NotNullNotDefaul()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			AssertHelpers.Throws<ArgumentException>(() => strategy.AddColumn(new Table("TestTable"), new Column("TestColumn", DbType.Int32, false)));
		}

		/// <summary>
		/// Добавление столбца с allow null
		/// </summary>
		[Test]
		public void AddWithNull()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.AddColumn(new Table("TestTable"), new Column("TestColumn", DbType.Int32));
			command.VerifySet(x => x.CommandText = "alter table [TestTable] add [TestColumn] int null");
		}

		/// <summary>
		/// Добавление столбца с дефалтом
		/// </summary>
		[Test]
		public void AddWithValueDefault()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.AddColumn(new Table("TestTable"), new Column("TestColumn", DbType.Int32, false) { Constraint = new ValueDefault("df_test", "100500") });
			command.VerifySet(x => x.CommandText = "alter table [TestTable] add [TestColumn] int not null constraint df_test default 100500");
		}

		/// <summary>
		/// Добавление столбца с дефалтом - время
		/// </summary>
		[Test]
		public void AddWithDateDefault()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.AddColumn(new Table("TestTable"), new Column("TestColumn", DbType.Int32, false) { Constraint = new StandartDefault("df_test", StandartDefaultType.date) });
			command.VerifySet(x => x.CommandText = "alter table [TestTable] add [TestColumn] int not null constraint df_test default getdate()");
		}

		/// <summary>
		/// Добавление столбца с дефалтом - GUID
		/// </summary>
		[Test]
		public void AddWithGuidDefault()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.AddColumn(new Table("TestTable"), new Column("TestColumn", DbType.Int32, false) { Constraint = new StandartDefault("df_test", StandartDefaultType.guid) });
			command.VerifySet(x => x.CommandText = "alter table [TestTable] add [TestColumn] int not null constraint df_test default newid()");
		}
	}
}