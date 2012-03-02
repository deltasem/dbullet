//-----------------------------------------------------------------------
// <copyright file="MsSql2008CreateIndexTest.cs" company="delta">
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
	/// Тесты создания индексов
	/// </summary>
	[TestFixture]
	public class MsSql2008CreateIndexTest : CreateIndexTest
	{
		/// <summary>
		/// Создание индекса
		/// </summary>
		protected override string RegularCreateIndexCommand
		{
			get
			{
				return "create nonclustered index [INDEX_NAME]" +
				       " on [TABLE_NAME] ([column4index] asc)" +
				       " with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)";
			}
		}

		/// <summary>
		/// Создание индекса оп убыванию
		/// </summary>
		protected override string DescCommad
		{
			get
			{
				return "create nonclustered index [INDEX_NAME]" +
				       " on [TABLE_NAME]" +
				       " ([column4index] desc)" +
				       " with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)";
			}
		}

		/// <summary>
		/// Создание индекса в нестандартной партиции
		/// </summary>
		protected override string PartitionalCommand
		{
			get
			{
				return "create nonclustered index [INDEX_NAME]" +
				       " on [TABLE_NAME] ([column4index] asc)" +
				       " with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)" +
				       " ON [INDEX_PARTITION]";
			}
		}

		/// <summary>
		/// Создание индекса в нестандартной партиции
		/// </summary>
		protected override string ClusteredCommand
		{
			get
			{
				return "create clustered index [INDEX_NAME]" +
				       " on [TABLE_NAME] ([column4index] asc)" +
				       " with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)";
			}
		}

		/// <summary>
		/// Создание уникального индекса
		/// </summary>
		protected override string UniqueCommand
		{
			get
			{
				return "create unique nonclustered index [INDEX_NAME]" +
				       " on [TABLE_NAME] ([column4index] asc)" +
				       " with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)";
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