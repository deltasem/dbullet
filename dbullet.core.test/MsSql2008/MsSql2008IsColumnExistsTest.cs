//-----------------------------------------------------------------------
// <copyright file="MsSql2008IsColumnExistsTest.cs" company="delta">
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
	/// Тесты IsColumnExists
	/// </summary>
	[TestFixture]
	public class MsSql2008IsColumnExistsTest : IsColumnExistsTest
	{
		/// <summary>
		/// Инициализация
		/// </summary>
		[SetUp]
		public void SelfTestInitialize()
		{
			ObjectFactory.Initialize(x => x.For<IDatabaseStrategy>().Use<MsSql2008Strategy>());
			ObjectFactory.Inject(connection.Object);
		}

		/// <summary>
		/// Проверяет, существует ли колонка
		/// </summary>
		protected override string ByNameCommand
		{
			get
			{
				return "select count(*) from syscolumns where id = object_id(N'ExistingTable') and name = 'ExistingColumn'";
			}
		}
	}
}