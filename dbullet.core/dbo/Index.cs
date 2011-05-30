//-----------------------------------------------------------------------
// <copyright file="Index.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using dbullet.core.exception;

namespace dbullet.core.dbo
{
	/// <summary>
	/// ������
	/// </summary>
	public class Index : DatabaseObjectBase, IPartitionable
	{
		/// <summary>
		/// ��� �������
		/// </summary>
		private readonly IndexType indexType;

		/// <summary>
		/// ������������ �������
		/// </summary>
		private readonly bool isUnique;

		/// <summary>
		/// �������� ��������
		/// </summary>
		private readonly string partitionName;

		/// <summary>
		/// �������
		/// </summary>
		private readonly Table table;

		/// <summary>
		/// ������� ��� �������
		/// </summary>
		private readonly List<IndexColumn> columns;

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="table">�������</param>
		/// <param name="partitionName">��������</param>
		/// <param name="indexType">��� �������</param>
		/// <param name="isUnique">���������� ������</param>
		public Index(string name, string table, string partitionName = "PRIMARY", IndexType indexType = IndexType.Nonclustered, bool isUnique = false)
			: this(name, new Table(table), new IndexColumn[0], partitionName, indexType, isUnique)
		{
		}

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="table">�������</param>
		/// <param name="partitionName">��������</param>
		/// <param name="indexType">��� �������</param>
		/// <param name="isUnique">���������� ������</param>
		public Index(string name, Table table, string partitionName = "PRIMARY", IndexType indexType = IndexType.Nonclustered, bool isUnique = false)
			: this(name, table, new IndexColumn[0], partitionName, indexType, isUnique)
		{
		}

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="table">�������</param>
		/// <param name="columns">������� ��� �������</param>
		/// <param name="partitionName">��������</param>
		/// <param name="indexType">��� �������</param>
		/// <param name="isUnique">���������� ������</param>
		public Index(string name, string table, IEnumerable<IndexColumn> columns, string partitionName = "PRIMARY", IndexType indexType = IndexType.Nonclustered, bool isUnique = false)
			: this(name, new Table(table), columns, partitionName, indexType, isUnique)
		{
		}

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="table">�������</param>
		/// <param name="columns">������� ��� �������</param>
		/// <param name="partitionName">��������</param>
		/// <param name="indexType">��� �������</param>
		/// <param name="isUnique">���������� ������</param>
		public Index(string name, Table table, IEnumerable<IndexColumn> columns, string partitionName = "PRIMARY", IndexType indexType = IndexType.Nonclustered, bool isUnique = false)
			: base(name)
		{
			this.table = table;
			this.columns = new List<IndexColumn>(columns);
			this.indexType = indexType;
			this.partitionName = partitionName;
			this.isUnique = isUnique;
		}

		/// <summary>
		/// ��� �������
		/// </summary>
		public IndexType IndexType
		{
			get { return indexType; }
		}

		/// <summary>
		/// ������������ �������
		/// </summary>
		public bool IsUnique
		{
			get { return isUnique; }
		}

		/// <summary>
		/// ������� ��� �������
		/// </summary>
		public List<IndexColumn> Columns
		{
			get { return columns; }
		}

		/// <summary>
		/// �������� ��������
		/// </summary>
		public string PartitionName
		{
			get { return partitionName; }
		}

		/// <summary>
		/// �������
		/// </summary>
		public Table Table
		{
			get { return table; }
		}

		/// <summary>
		/// �������� ������� � �������
		/// </summary>
		/// <param name="column">�������</param>
		/// <returns>������, � ���������� ��������</returns>
		public Index AddColumn(IndexColumn column)
		{
			if (columns.FirstOrDefault(p => p.Name == column.Name) != null)
			{
				throw new DublicateColumnException();
			}

			columns.Add(column);
			return this;
		}
	}
}