//-----------------------------------------------------------------------
// <copyright file="MsSql2008InsertRowsTest.cs" company="delta">
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
	/// Тесты InsertRows
	/// </summary>
	[TestFixture]
	public class MsSql2008InsertRowsTest : InsertRowsTest
	{
		/// <summary>
		/// Обычная вставк записи
		/// </summary>
		protected override string RegularInsertCommand
		{
			get { return "insert into [testtable] ([FIELD_1], [FIELD_2]) values('1', '2');"; }
		}

		/// <summary>
		/// Обычная вставк записи
		/// </summary>
		protected override string InsertTwoRowCommand
		{
			get { return "insert into [testtable] ([FIELD_1], [FIELD_2]) values('3', '4');"; }
		}

		/// <summary>
		/// Если указано identity, то должно использоваться
		/// </summary>
		protected override string InsertShoudUseIdentityCommand
		{
			get
			{
				return "set identity_insert [testtable] on; insert into [testtable] ([FIELD_1], [FIELD_2]) values('1', '2'); set identity_insert [testtable] off;";
			}
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