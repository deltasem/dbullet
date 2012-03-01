//-----------------------------------------------------------------------
// <copyright file="CreateTableTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Data;
using dbullet.core.dbo;
using dbullet.core.dbs;
using dbullet.core.exception;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// ����� CreateTable
	/// </summary>
	[TestFixture]
	public abstract class CreateTableTest : TestBase
	{
		/// <summary>
		/// ���������� ��������
		/// </summary>
		protected abstract string RegularCreateTableCommand { get; }

		/// <summary>
		/// ���������� ��������
		/// </summary>
		protected abstract string RegularCreateTableWithIdentityCommand { get; }

		/// <summary>
		/// ���������� �������� � ������ ��������
		/// </summary>
		protected abstract string CustomPartitionCommand { get; }

		/// <summary>
		/// ���������� �������� � ������ �������� c ��������� ������
		/// </summary>
		protected abstract string CustomPartitionWithPrimaryKeyCommand { get; }

		/// <summary>
		/// �������� ������� � ��������� ������ � �� ����������� ��������
		/// </summary>
		protected abstract string WithPrimaryKeyCustomPartitionCommand { get; }

		/// <summary>
		/// �������� � ��������
		/// </summary>
		protected abstract string WithDefaultCommand { get; }

		/// <summary>
		/// �� ����������� �������� - ��������� �����
		/// </summary>
		protected abstract string WithStandartDefaultDateCommand { get; }

		/// <summary>
		/// �� ����������� �������� - ����� GUID
		/// </summary>
		protected abstract string WithStandartDefaultGuidCommand { get; }

		/// <summary>
		/// �������� ������� ��� ��������
		/// </summary>
		[Test]
		public void WithoutCollumns()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			var table = new Table("TestTable");
			Assert.Throws<ColumnExpectedException>(() => strategy.CreateTable(table));
		}

		/// <summary>
		/// ���������� ��������
		/// </summary>
		[Test]
		public void RegularCreateTable()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			var table = new Table("TestTable");
			table.AddColumn(new Column("test", DbType.Int32));
			table.AddColumn(new Column("test2", DbType.String.Size(50)));
			strategy.CreateTable(table);
			command.VerifySet(x => x.CommandText = RegularCreateTableCommand);
		}

		/// <summary>
		/// ���������� �������� � ��������
		/// </summary>
		[Test]
		public void RegularCreateTableWithIdentity()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			var table = new Table("TestTable");
			table.AddColumn(new Column("test", DbType.Int32, false, true));
			table.AddColumn(new Column("test2", DbType.String.Size(50)));
			strategy.CreateTable(table);
			command.VerifySet(x => x.CommandText = RegularCreateTableWithIdentityCommand);
		}

		/// <summary>
		/// ���������� �������� � ������ ��������
		/// </summary>
		[Test]
		public void CustomPartition()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			var table = new Table("TestTable", "TESTPARTIOTION")
				.AddColumn(new Column("test", DbType.Int32))
				.AddColumn(new Column("test2", DbType.String.Size(50)));
			strategy.CreateTable(table);
			command.VerifySet(x => x.CommandText = CustomPartitionCommand);
		}

		/// <summary>
		/// ���������� �������� � ������ �������� c ��������� ������
		/// </summary>
		[Test]
		public void CustomPartitionWithPrimaryKey()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			var table = new Table(
				"TestTable", "TESTPARTIOTION")
				.AddColumn(new Column("testid", DbType.Int32))
				.AddColumn(new Column("test2", DbType.String.Size(50)))
				.AddPrimaryKey("testid");
			strategy.CreateTable(table);
			command.VerifySet(x => x.CommandText = CustomPartitionWithPrimaryKeyCommand);
		}

		/// <summary>
		/// �������� ������� � ��������� ������ � �� ����������� ��������
		/// </summary>
		[Test]
		public void WithPrimaryKeyCustomPartition()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			var table = new Table(
				"TestTable")
				.AddColumn(new Column("testid", DbType.Int32))
				.AddColumn(new Column("test2", DbType.String.Size(50)))
				.AddPrimaryKey("testid", "TESTPARTIOTION");
			strategy.CreateTable(table);
			command.VerifySet(x => x.CommandText = WithPrimaryKeyCustomPartitionCommand);
		}

		/// <summary>
		/// ���������� ��������
		/// </summary>
		[Test]
		public void WithDefault()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			var table = new Table("TestTable");
			table.AddColumn(new Column("test", DbType.Int32)).Default("100500");
			table.AddColumn(new Column("test2", DbType.String.Size(50))).Default("this is the test");
			strategy.CreateTable(table);
			command.VerifySet(x => x.CommandText = WithDefaultCommand);
		}

		/// <summary>
		/// �� ����������� �������� - ��������� �����
		/// </summary>
		[Test]
		public void WithStandartDefaultDate()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			var table = new Table("TestTable");
			table.AddColumn(new Column("test", DbType.Int32)).Default(StandartDefaultType.date);
			strategy.CreateTable(table);
			command.VerifySet(x => x.CommandText = WithStandartDefaultDateCommand);
		}

		/// <summary>
		/// �� ����������� �������� - ����� GUID
		/// </summary>
		[Test]
		public void WithStandartDefaultGuid()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			var table = new Table("TestTable");
			table.AddColumn(new Column("test", DbType.Int32)).Default(StandartDefaultType.guid);
			strategy.CreateTable(table);
			command.VerifySet(x => x.CommandText = WithStandartDefaultGuidCommand);
		}
	}
}