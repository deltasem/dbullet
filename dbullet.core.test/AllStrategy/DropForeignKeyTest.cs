//-----------------------------------------------------------------------
// <copyright file="DropForeignKeyTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbo;
using dbullet.core.dbs;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// Тесты DropForeignKey
	/// </summary>
	[TestFixture]
	public abstract class DropForeignKeyTest : TestBase
	{
		/// <summary>
		/// Удаление внешнего ключа
		/// </summary>
		[Test]
		public void RegularDropForeignKey()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			strategy.DropForeignKey(new ForeignKey("FK_TEST", "TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2", ForeignAction.NoAction));
			command.VerifySet(x => x.CommandText = "alter table [TABLE1] drop constraint FK_TEST");
		}
	}
}