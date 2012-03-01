//-----------------------------------------------------------------------
// <copyright file="IndexTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using dbullet.core.dbo;
using dbullet.core.exception;
using NUnit.Framework;

namespace dbullet.core.test
{
	/// <summary>
	/// Тесты для индексов
	/// </summary>
	[TestFixture]
	public class IndexTest
	{
		/// <summary>
		/// Добавление колонки возвращает тот же индекс
		/// </summary>
		[Test]
		public void AddColumnSameObjectTest()
		{
			var idx = new Index("IDX_TEST", "testTable");
			var idx2 = idx.AddColumn(new IndexColumn("testcol"));
			Assert.AreSame(idx, idx2);
		}

		/// <summary>
		/// Добавление колонки
		/// </summary>
		[Test]
		public void AddColumnTest()
		{
			var idx = new Index("IDX_TEST", "testTable");
			idx.AddColumn(new IndexColumn("Test"));
			Assert.AreEqual(1, idx.Columns.Count);
		}

		/// <summary>
		/// Добавление два раза одной и той же колонки должно вызвать исключение
		/// </summary>
		[Test]
		public void AddColumnDublicateTest()
		{
			var idx = new Index("IDX_TEST", "testTable");
			idx.AddColumn(new IndexColumn("Test"));
			Assert.Throws<DublicateColumnException>(() => idx.AddColumn(new IndexColumn("Test")));
		}
	}
}