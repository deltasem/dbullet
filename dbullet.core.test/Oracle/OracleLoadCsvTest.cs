//-----------------------------------------------------------------------
// <copyright file="OracleLoadCsvTest.cs" company="delta" author="delta">
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
	/// ����� �������� CSV
	/// </summary>
	[TestFixture]
	public class OracleLoadCsvTest : LoadCsvTest
	{
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