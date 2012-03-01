//-----------------------------------------------------------------------
// <copyright file="MsSql2008IsTableExistsTest.cs" company="delta">
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
	/// Тесты IsTableExistsTest
	/// </summary>
	[TestFixture]
	public class MsSql2008IsTableExistsTest : AllStrategy.IsTableExistsTest
	{
		/// <summary>
		/// Проверяет, существует ли таблица
		/// </summary>
		protected override string ByNameCommandText
		{
			get { return "select count(*) from sysobjects where id = object_id(N'ExistingTable') and OBJECTPROPERTY(id, N'IsTable') = 1"; }
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