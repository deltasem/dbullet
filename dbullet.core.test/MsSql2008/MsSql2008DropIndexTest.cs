﻿//-----------------------------------------------------------------------
// <copyright file="MsSql2008DropIndexTest.cs" company="delta">
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
	/// Тест удаления индексов
	/// </summary>
	[TestFixture]
	public class MsSql2008DropIndexTest : DropIndexTest
	{
		/// <summary>
		/// Удаление индекса
		/// </summary>
		protected override string RegularDropIndexCommand
		{
			get { return "drop index INDEX_NAME on [TABLE_NAME]"; }
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