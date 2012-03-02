//-----------------------------------------------------------------------
// <copyright file="MsSql2008AddColumnTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbs;
using dbullet.core.engine.MsSql;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.MsSql2008
{
	/// <summary>
	/// ����� ���������� �������
	/// </summary>
	[TestFixture]
	public class MsSql2008AddColumnTest : AllStrategy.AddColumnTest
	{
		/// <summary>
		/// ���������� ������� � allow null
		/// </summary>
		protected override string AddWithNullCommand
		{
			get { return "alter table [TestTable] add [TestColumn] int null"; }
		}

		/// <summary>
		/// ���������� ������� � ��������
		/// </summary>
		protected override string AddWithValueDefaultCommand
		{
			get { return "alter table [TestTable] add [TestColumn] int not null constraint df_test default 100500"; }
		}

		/// <summary>
		/// ���������� ������� � �������� - �����
		/// </summary>
		protected override string AddWithDateDefaultCommand
		{
			get { return "alter table [TestTable] add [TestColumn] int not null constraint df_test default getdate()"; }
		}

		/// <summary>
		/// ���������� ������� � �������� - GUID
		/// </summary>
		protected override string AddWithGuidDefaultCommand
		{
			get { return "alter table [TestTable] add [TestColumn] int not null constraint df_test default newid()"; }
		}

		/// <summary>
		/// �������������
		/// </summary>
		[SetUp]
		public void SelfTestInitialize()
		{
			ObjectFactory.Initialize(x => x.For<IDatabaseStrategy>().Use<MsSql2008Strategy>());
			ObjectFactory.Inject(connection.Object);
		}
	}
}