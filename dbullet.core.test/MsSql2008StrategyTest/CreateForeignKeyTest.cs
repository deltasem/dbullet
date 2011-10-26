//-----------------------------------------------------------------------
// <copyright file="CreateForeignKeyTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using dbullet.core.dbo;
using dbullet.core.engine;
using dbullet.core.test.tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// Тесты создания внешних ключей
	/// </summary>
	[TestClass]
	public class CreateForeignKeyTest
	{
		/// <summary>
		/// Создание внешнего ключа
		/// </summary>
		[TestMethod]
		public void RegularCreateFK()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateForeignKey(new ForeignKey("FK_TEST", "TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2"));
			Assert.AreEqual("alter table [TABLE1] add constraint FK_TEST foreign key ([ID_TABLE1]) references [TABLE2] ([ID_TABLE2]) on update no action on delete set null", connection.LastCommandText);
		}

		/// <summary>
		/// Создание внешнего ключа имя автогенерируемое
		/// </summary>
		[TestMethod]
		public void DefaultName()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateForeignKey(new ForeignKey("TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2"));
			Assert.AreEqual("alter table [TABLE1] add constraint FK_TABLE1_TABLE2 foreign key ([ID_TABLE1]) references [TABLE2] ([ID_TABLE2]) on update no action on delete set null", connection.LastCommandText);
		}

		/// <summary>
		/// Создание внешнего ключа имя автогенерируемое
		/// </summary>
		[TestMethod]
		public void Cascade()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateForeignKey(new ForeignKey("FK_TEST", "TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2", ForeignAction.Cascade));
			Assert.AreEqual("alter table [TABLE1] add constraint FK_TEST foreign key ([ID_TABLE1]) references [TABLE2] ([ID_TABLE2]) on update no action on delete cascade", connection.LastCommandText);
		}

		/// <summary>
		/// Создание внешнего ключа имя автогенерируемое
		/// </summary>
		[TestMethod]
		public void NoAction()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateForeignKey(new ForeignKey("FK_TEST", "TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2", ForeignAction.NoAction));
			Assert.AreEqual("alter table [TABLE1] add constraint FK_TEST foreign key ([ID_TABLE1]) references [TABLE2] ([ID_TABLE2]) on update no action on delete no action", connection.LastCommandText);
		}		
	}
}