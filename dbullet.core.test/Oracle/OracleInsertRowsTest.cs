//-----------------------------------------------------------------------
// <copyright file="OracleInsertRowsTest.cs" company="delta" author="delta">
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
	/// Тесты InsertRows
	/// </summary>
	[TestFixture]
	public class OracleInsertRowsTest : InsertRowsTest
	{
		/// <summary>
		/// Обычная вставк записи
		/// </summary>
		protected override string RegularInsertCommand
		{
			get { return "insert into \"TESTTABLE\" (\"FIELD_1\", \"FIELD_2\") values('1', '2')"; }
		}

		/// <summary>
		/// Обычная вставк записи
		/// </summary>
		protected override string InsertTwoRowCommand
		{
			get { return "insert into \"TESTTABLE\" (\"FIELD_1\", \"FIELD_2\") values('3', '4')"; }
		}

		/// <summary>
		/// Если указано identity, то должно использоваться
		/// </summary>
		protected override string InsertShoudUseIdentityCommand
		{
			get
			{
				return "insert into \"TESTTABLE\" (\"FIELD_1\", \"FIELD_2\") values('1', '2')";
			}
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