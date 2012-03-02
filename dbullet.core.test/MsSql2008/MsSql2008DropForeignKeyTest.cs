//-----------------------------------------------------------------------
// <copyright file="MsSql2008DropForeignKeyTest.cs" company="delta">
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
	/// Тесты удаления внешних ключей
	/// </summary>
	[TestFixture]
	public class MsSql2008DropForeignKeyTest : DropForeignKeyTest
	{
		/// <summary>
		/// Удаление внешнего ключа
		/// </summary>
		protected override string RegularDropForeignKeyCommand
		{
			get { return "alter table [TABLE1] drop constraint FK_TEST"; }
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