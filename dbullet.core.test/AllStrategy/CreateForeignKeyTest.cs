//-----------------------------------------------------------------------
// <copyright file="CreateForeignKeyTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbo;
using dbullet.core.dbs;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// Тесты создания внешних ключей
	/// </summary>
	[TestFixture]
	public abstract class CreateForeignKeyTest : TestBase
	{
		/// <summary>
		/// Создание внешнего ключа
		/// </summary>
		[Test]
		public void RegularCreateFK()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateForeignKey(new ForeignKey("FK_TEST", "TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2"));
			command.VerifySet(x => x.CommandText = "alter table [TABLE1] add constraint FK_TEST foreign key ([ID_TABLE1]) references [TABLE2] ([ID_TABLE2]) on update no action on delete set null");
		}

		/// <summary>
		/// Создание внешнего ключа имя автогенерируемое
		/// </summary>
		[Test]
		public void DefaultName()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateForeignKey(new ForeignKey("TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2"));
			command.VerifySet(x => x.CommandText = "alter table [TABLE1] add constraint FK_TABLE1_TABLE2 foreign key ([ID_TABLE1]) references [TABLE2] ([ID_TABLE2]) on update no action on delete set null");
		}

		/// <summary>
		/// Создание внешнего ключа имя автогенерируемое
		/// </summary>
		[Test]
		public void Cascade()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateForeignKey(new ForeignKey("FK_TEST", "TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2", ForeignAction.Cascade));
			command.VerifySet(x => x.CommandText = "alter table [TABLE1] add constraint FK_TEST foreign key ([ID_TABLE1]) references [TABLE2] ([ID_TABLE2]) on update no action on delete cascade");
		}

		/// <summary>
		/// Создание внешнего ключа имя автогенерируемое
		/// </summary>
		[Test]
		public void NoAction()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateForeignKey(new ForeignKey("FK_TEST", "TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2", ForeignAction.NoAction));
			command.VerifySet(x => x.CommandText = "alter table [TABLE1] add constraint FK_TEST foreign key ([ID_TABLE1]) references [TABLE2] ([ID_TABLE2]) on update no action on delete no action");
		}
	}
}