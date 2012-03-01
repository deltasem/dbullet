//-----------------------------------------------------------------------
// <copyright file="OracleGetDefaultValueTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbo;
using dbullet.core.dbs;
using dbullet.core.engine.MsSql;
using dbullet.core.engine.Oracle;
using dbullet.core.test.AllStrategy;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.Oracle
{
	/// <summary>
	/// ����� ��������� �������� �������
	/// </summary>
	[TestFixture]
	public class OracleGetDefaultValueTest : GetDefaultValueTest
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

		/// <summary>
		/// ������-��������
		/// </summary>
		[Test]
		public override void ValueDefault()
		{
			var result = MsSqlTemplateManager.GetDefaultValue(new ValueDefault(string.Empty, "Value default"));
			Assert.AreEqual("Value default", result);
		}

		/// <summary>
		/// ������-������� �����
		/// </summary>
		[Test]
		public override void StandartDefaultDate()
		{
			var result = MsSqlTemplateManager.GetDefaultValue(new StandartDefault(string.Empty, StandartDefaultType.date));
			Assert.AreEqual("getdate()", result);
		}

		/// <summary>
		/// ������-����� GUID
		/// </summary>
		[Test]
		public override void StandartDefaultGuid()
		{
			var result = MsSqlTemplateManager.GetDefaultValue(new StandartDefault(string.Empty, StandartDefaultType.guid));
			Assert.AreEqual("newid()", result);
		}
	}
}