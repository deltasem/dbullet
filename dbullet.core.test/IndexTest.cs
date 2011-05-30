//-----------------------------------------------------------------------
// <copyright file="IndexTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using dbullet.core.dbo;
using dbullet.core.exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test
{
	/// <summary>
	/// ����� ��� ��������
	/// </summary>
	[TestClass]
	public class IndexTest
	{
		/// <summary>
		/// ���������� ������� ���������� ��� �� ������
		/// </summary>
		[TestMethod]
		public void AddColumnSameObjectTest()
		{
			var idx = new Index("IDX_TEST", "testTable");
			var idx2 = idx.AddColumn(new IndexColumn("testcol"));
			Assert.AreSame(idx, idx2);
		}

		/// <summary>
		/// ���������� �������
		/// </summary>
		[TestMethod]
		public void AddColumnTest()
		{
			var idx = new Index("IDX_TEST", "testTable");
			idx.AddColumn(new IndexColumn("Test"));
			Assert.AreEqual(1, idx.Columns.Count);
		}

		/// <summary>
		/// ���������� ��� ���� ����� � ��� �� ������� ������ ������� ����������
		/// </summary>
		[TestMethod]
		public void AddColumnDublicateTest()
		{
			var idx = new Index("IDX_TEST", "testTable");
			idx.AddColumn(new IndexColumn("Test"));
			AssertHelpers.Throws<DublicateColumnException>(() => idx.AddColumn(new IndexColumn("Test")));
		}
	}
}