//-----------------------------------------------------------------------
// <copyright file="MsSql2008CreateForeignKeyTest.cs" company="delta">
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
	/// Тесты создания внешних ключей
	/// </summary>
	[TestFixture]
	public class MsSql2008CreateForeignKeyTest : CreateForeignKeyTest
	{
		/// <summary>
		/// Создание внешнего ключа
		/// </summary>
		protected override string RegularCreateFKCommand
		{
			get
			{
				return "alter table" +
				       " [TABLE1] add constraint FK_TEST foreign key ([ID_TABLE1])" +
				       " references [TABLE2] ([ID_TABLE2]) on update no action on delete set null";
			}
		}

		/// <summary>
		/// Создание внешнего ключа имя автогенерируемое
		/// </summary>
		protected override string DefaultNameCommand
		{
			get
			{
				return "alter table [TABLE1]" +
				       " add constraint FK_TABLE1_TABLE2 foreign key ([ID_TABLE1])" +
				       " references [TABLE2] ([ID_TABLE2]) on update no action on delete set null";
			}
		}

		/// <summary>
		/// Создание внешнего ключа каскадное удаление
		/// </summary>
		protected override string CascadeCommand
		{
			get
			{
				return "alter table [TABLE1]" +
				       " add constraint FK_TEST foreign key ([ID_TABLE1])" +
				       " references [TABLE2] ([ID_TABLE2]) on update no action on delete cascade";
			}
		}

		/// <summary>
		/// Создание внешнего ключа без действия
		/// </summary>
		protected override string NoActionCommand
		{
			get
			{
				return "alter table [TABLE1]" +
				       " add constraint FK_TEST foreign key ([ID_TABLE1])" +
				       " references [TABLE2] ([ID_TABLE2]) on update no action on delete no action";
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