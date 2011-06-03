//-----------------------------------------------------------------------
// <copyright file="CreateIndexTest.cs" company="delta">
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
	/// Тесты создания индексов
	/// </summary>
	[TestClass]
	public class CreateIndexTest
	{
		/// <summary>
		/// Создание индекса
		/// </summary>
		[TestMethod]
		public void RegularCreateIndex()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }));
			Assert.AreEqual("create nonclustered index INDEX_NAME on TABLE_NAME (column4index asc) with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", connection.LastCommandText);
		}

		/// <summary>
		/// Создание индекса оп убыванию
		/// </summary>
		[TestMethod]
		public void Desc()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index", Direction.Descending) }));
			Assert.AreEqual("create nonclustered index INDEX_NAME on TABLE_NAME (column4index desc) with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", connection.LastCommandText);
		}

		/// <summary>
		/// Создание индекса в нестандартной партиции
		/// </summary>
		[TestMethod]
		public void Partitional()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }, "INDEX_PARTITION"));
			Assert.AreEqual("create nonclustered index INDEX_NAME on TABLE_NAME (column4index asc) with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX_PARTITION]", connection.LastCommandText);
		}

		/// <summary>
		/// Создание индекса в нестандартной партиции
		/// </summary>
		[TestMethod]
		public void Clustered()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }, "PRIMARY", IndexType.Clustered));
			Assert.AreEqual("create clustered index INDEX_NAME on TABLE_NAME (column4index asc) with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", connection.LastCommandText);
		}

		/// <summary>
		/// Создание уникального индекса
		/// </summary>
		[TestMethod]
		public void Unique()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }, "PRIMARY", IndexType.Nonclustered, true));
			Assert.AreEqual("create unique nonclustered index INDEX_NAME on TABLE_NAME (column4index asc) with (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", connection.LastCommandText);
		}		
	}
}