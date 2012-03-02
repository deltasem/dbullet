//-----------------------------------------------------------------------
// <copyright file="OracleDropForeignKeyTest.cs" company="delta" author="delta">
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
	/// ����� �������� ������� ������
	/// </summary>
	[TestFixture]
	public class OracleDropForeignKeyTest : DropForeignKeyTest
	{
		/// <summary>
		/// �������� �������� �����
		/// </summary>
		protected override string RegularDropForeignKeyCommand
		{
			get { return "alter table \"TABLE1\" drop constraint \"FK_TEST\""; }
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