//-----------------------------------------------------------------------
// <copyright file="OracleDropIndexTest.cs" company="delta" author="delta">
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
	/// ���� �������� ��������
	/// </summary>
	[TestFixture]
	public class OracleDropIndexTest : DropIndexTest
	{
		/// <summary>
		/// �������� �������
		/// </summary>
		protected override string RegularDropIndexCommand
		{
			get { return "drop index \"INDEX_NAME\""; }
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