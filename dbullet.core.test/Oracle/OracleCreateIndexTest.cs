//-----------------------------------------------------------------------
// <copyright file="OracleCreateIndexTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbs;
using dbullet.core.engine.Oracle;
using dbullet.core.test.AllStrategy;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.Oracle
{
	/// <summary>
	/// Тесты создания индексов
	/// </summary>
	[TestFixture]
	public class OracleCreateIndexTest : CreateIndexTest
	{
		/// <summary>
		/// Создание индекса
		/// </summary>
		protected override string RegularCreateIndexCommand
		{
			get
			{
				return "create index \"INDEX_NAME\"" +
							 " on \"TABLE_NAME\" (\"COLUMN4INDEX\")";
			}
		}

		/// <summary>
		/// Создание индекса оп убыванию
		/// </summary>
		protected override string DescCommad
		{
			get
			{
				return "create index \"INDEX_NAME\"" +
							 " on \"TABLE_NAME\" (\"COLUMN4INDEX\")";
			}
		}

		/// <summary>
		/// Создание индекса в нестандартной партиции
		/// </summary>
		protected override string PartitionalCommand
		{
			get
			{
				return "create index \"INDEX_NAME\"" +
							 " on \"TABLE_NAME\" (\"COLUMN4INDEX\")" +
							 " tablespace \"INDEX_PARTITION\"";
			}
		}

		/// <summary>
		/// Создание индекса в нестандартной партиции
		/// </summary>
		protected override string ClusteredCommand
		{
			get
			{
				return "create index \"INDEX_NAME\"" +
							 " on \"TABLE_NAME\" (\"COLUMN4INDEX\")";
			}
		}

		/// <summary>
		/// Создание уникального индекса
		/// </summary>
		protected override string UniqueCommand
		{
			get
			{
				return "create unique index \"INDEX_NAME\"" +
							 " on \"TABLE_NAME\" (\"COLUMN4INDEX\")";
			}
		}

		/// <summary>
		/// Инициализация
		/// </summary>
		[SetUp]
		public void SelfTestInitialize()
		{
			ObjectFactory.Initialize(x => x.For<IDatabaseStrategy>().Use<OracleStrategy>());
			ObjectFactory.Inject(connection.Object);
		}
	}
}