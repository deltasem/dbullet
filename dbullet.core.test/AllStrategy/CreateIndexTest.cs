//-----------------------------------------------------------------------
// <copyright file="CreateIndexTest.cs" company="delta" author="delta">
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
	/// Тесты CreateIndex
	/// </summary>
	[TestFixture]
	public class CreateIndexTest : TestBase
	{
		/// <summary>
		/// Создание индекса
		/// </summary>
		[Test]
		public void RegularCreateIndex()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }));
			command.VerifySet(x => x.CommandText = "create nonclustered index [INDEX_NAME] on [TABLE_NAME] ([column4index] asc) with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
		}

		/// <summary>
		/// Создание индекса оп убыванию
		/// </summary>
		[Test]
		public void Desc()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index", Direction.Descending) }));
			command.VerifySet(x => x.CommandText = "create nonclustered index [INDEX_NAME] on [TABLE_NAME] ([column4index] desc) with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
		}

		/// <summary>
		/// Создание индекса в нестандартной партиции
		/// </summary>
		[Test]
		public void Partitional()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }, "INDEX_PARTITION"));
			command.VerifySet(x => x.CommandText = "create nonclustered index [INDEX_NAME] on [TABLE_NAME] ([column4index] asc) with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX_PARTITION]");
		}

		/// <summary>
		/// Создание индекса в нестандартной партиции
		/// </summary>
		[Test]
		public void Clustered()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }, "PRIMARY", IndexType.Clustered));
			command.VerifySet(x => x.CommandText = "create clustered index [INDEX_NAME] on [TABLE_NAME] ([column4index] asc) with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
		}

		/// <summary>
		/// Создание уникального индекса
		/// </summary>
		[Test]
		public void Unique()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }, "PRIMARY", IndexType.Nonclustered, true));
			command.VerifySet(x => x.CommandText = "create unique nonclustered index [INDEX_NAME] on [TABLE_NAME] ([column4index] asc) with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
		}
	}
}