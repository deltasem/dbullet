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
		protected abstract string RegularCreateFKCommand { get; }

		/// <summary>
		/// Создание внешнего ключа имя автогенерируемое
		/// </summary>
		protected abstract string DefaultNameCommand { get; }

		/// <summary>
		/// Создание внешнего ключа каскадное удаление
		/// </summary>
		protected abstract string CascadeCommand { get; }

		/// <summary>
		/// Создание внешнего ключа без действия
		/// </summary>
		protected abstract string NoActionCommand { get; }

		/// <summary>
		/// Создание внешнего ключа
		/// </summary>
		[Test]
		public void RegularCreateFK()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateForeignKey(new ForeignKey("FK_TEST", "TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2"));
			command.VerifySet(x => x.CommandText = RegularCreateFKCommand);
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
			command.VerifySet(x => x.CommandText = DefaultNameCommand);
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
			command.VerifySet(x => x.CommandText = CascadeCommand);
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
			command.VerifySet(x => x.CommandText = NoActionCommand);
		}
	}
}