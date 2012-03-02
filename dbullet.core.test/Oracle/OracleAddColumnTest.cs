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
	/// ����� ���������� �������
	/// </summary>
	[TestFixture]
	public class OracleAddColumnTest : AllStrategy.AddColumnTest
	{
		/// <summary>
		/// ���������� ������� � allow null
		/// </summary>
		protected override string AddWithNullCommand
		{
			get { return "begin execute immediate 'alter table \"TESTTABLE\" add \"TESTCOLUMN\" int null'; end;"; }
		}

		/// <summary>
		/// ���������� ������� � ��������
		/// </summary>
		protected override string AddWithValueDefaultCommand
		{
			get { return "begin execute immediate 'alter table \"TESTTABLE\" add \"TESTCOLUMN\" int default ''100500'' not null'; end;"; }
		}

		/// <summary>
		/// ���������� ������� � �������� - �����
		/// </summary>
		protected override string AddWithDateDefaultCommand
		{
			get { return "begin execute immediate 'alter table \"TESTTABLE\" add \"TESTCOLUMN\" int default sysdate not null'; end;"; }
		}

		/// <summary>
		/// ���������� ������� � �������� - GUID
		/// </summary>
		protected override string AddWithGuidDefaultCommand
		{
			get { return "begin execute immediate 'alter table \"TESTTABLE\" add \"TESTCOLUMN\" int default sys_guid() not null'; end;"; }
		}

		/// <summary>
		/// �������������
		/// </summary>
		[SetUp]
		public void SelfTestInitialize()
		{
			ObjectFactory.Initialize(x => x.For<IDatabaseStrategy>().Use<OracleStrategy>());
			ObjectFactory.Inject(connection.Object);
		}
	}
}