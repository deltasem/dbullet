//-----------------------------------------------------------------------
// <copyright file="ExecuteQueryTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Data;
using dbullet.core.engine;
using dbullet.core.engine.common;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// тесты ExecuteQuery
	/// </summary>
	[TestFixture]
	public class ExecuteQueryTest : TestBase
	{
		/// <summary>
		/// Менеджер темплейтов
		/// </summary>
		private Mock<ITemplateManager> templateManager = new Mock<ITemplateManager>();

		/// <summary>
		/// Инициализация
		/// </summary>
		[SetUp]
		public void SelfTestInitialize()
		{
			ObjectFactory.Initialize(x => { });
			ObjectFactory.Inject(connection.Object);
			ObjectFactory.Inject(templateManager.Object);
		}

		/// <summary>
		/// Не должен вызваться запрос, если разные стратегии заявленные и указанные
		/// </summary>
		[Test]
		public void ShouldNotExecuteQueryForDiferentStrategy()
		{
			var strategy = ObjectFactory.GetInstance<FakeStrategy>();
			strategy.CurrentStrategy = SupportedStrategy.Oracle;
			strategy.ExecuteQuery(SupportedStrategy.Mssql2008, "query");
			command.Verify(x => x.ExecuteNonQuery(), Times.Never());
		}

		/// <summary>
		/// Должен вызваться запрос, если совпадают стратегии заявленные и указанные
		/// </summary>
		[Test]
		public void ShouldExecuteQueryForSameStrategy()
		{
			var strategy = ObjectFactory.GetInstance<FakeStrategy>();
			strategy.CurrentStrategy = SupportedStrategy.Oracle;
			strategy.ExecuteQuery(SupportedStrategy.Oracle, "query");
			command.Verify(x => x.ExecuteNonQuery(), Times.Once());
		}

		/// <summary>
		/// Должен вызваться запрос, если стратегия Any
		/// </summary>
		[Test]
		public void ShouldExecuteQueryIfAnyStrategy()
		{
			var strategy = ObjectFactory.GetInstance<FakeStrategy>();
			strategy.CurrentStrategy = SupportedStrategy.Oracle;
			strategy.ExecuteQuery(SupportedStrategy.Any, "query");
			command.Verify(x => x.ExecuteNonQuery(), Times.Once());			
		}

		/// <summary>
		/// Тестовая стратегия
		/// </summary>
		public class FakeStrategy : StrategyBase
		{
			/// <summary>
			/// Конструктор
			/// </summary>
			/// <param name="connection">Соединение с БД</param>
			/// <param name="templateManager">Менеджер</param>
			public FakeStrategy(IDbConnection connection, ITemplateManager templateManager) : base(connection, templateManager)
			{
			}

			/// <summary>
			/// Стратегия
			/// </summary>
			public override SupportedStrategy Strategy
			{
				get { return CurrentStrategy; }
			}

			/// <summary>
			/// Текущая стратегия
			/// </summary>
			public SupportedStrategy CurrentStrategy { get; set; }

			/// <summary>
			/// identity_insert off
			/// </summary>
			/// <param name="tableName">Имя таблицы</param>
			/// <param name="cmd">Комманда</param>
			protected override void DisableIdentityInsert(string tableName, IDbCommand cmd)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>
			/// Возвращает имя параметра
			/// </summary>
			/// <param name="headers">Заголовки</param>
			/// <param name="i">ИД параметра</param>
			/// <returns>Имя параметра</returns>
			protected override string GetParameterName(string[] headers, int i)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>
			/// identity_insert on
			/// </summary>
			/// <param name="tableName">Имя таблицы</param>
			/// <param name="cmd">Комманда</param>
			protected override void EnableIdentityInsert(string tableName, IDbCommand cmd)
			{
				throw new System.NotImplementedException();
			}
		}
	}
}