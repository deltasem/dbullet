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
	public abstract class CreateIndexTest : TestBase
	{
		/// <summary>
		/// Создание индекса
		/// </summary>
		protected abstract string RegularCreateIndexCommand { get; }

		/// <summary>
		/// Создание индекса оп убыванию
		/// </summary>
		protected abstract string DescCommad { get; }

		/// <summary>
		/// Создание индекса в нестандартной партиции
		/// </summary>
		protected abstract string PartitionalCommand { get; }

		/// <summary>
		/// Создание индекса в нестандартной партиции
		/// </summary>
		protected abstract string ClusteredCommand { get; }

		/// <summary>
		/// Создание уникального индекса
		/// </summary>
		protected abstract string UniqueCommand { get; }

		/// <summary>
		/// Создание индекса
		/// </summary>
		[Test]
		public void RegularCreateIndex()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }));
			command.VerifySet(x => x.CommandText = RegularCreateIndexCommand);
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
			command.VerifySet(x => x.CommandText = DescCommad);
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
			command.VerifySet(x => x.CommandText = PartitionalCommand);
		}

		/// <summary>
		/// Создание индекса в нестандартной партиции
		/// </summary>
		[Test]
		public void Clustered()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }, string.Empty, IndexType.Clustered));
			command.VerifySet(x => x.CommandText = ClusteredCommand);
		}

		/// <summary>
		/// Создание уникального индекса
		/// </summary>
		[Test]
		public void Unique()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateIndex(new Index("INDEX_NAME", "TABLE_NAME", new[] { new IndexColumn("column4index") }, string.Empty, IndexType.Nonclustered, true));
			command.VerifySet(x => x.CommandText = UniqueCommand);
		}
	}
}