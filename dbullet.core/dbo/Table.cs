//-----------------------------------------------------------------------
// <copyright file="Table.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	using System.Collections.Generic;

	/// <summary>
	/// �������
	/// </summary>
	public class Table : DatabaseObjectBase, IPartitionable
	{
		/// <summary>
		/// �������
		/// </summary>
		private readonly List<Column> columns;

		/// <summary>
		/// �������� ��������
		/// </summary>
		private string partitionName;

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		public Table(string name) : this(name, "PRIMARY")
		{
		}

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="partitionName">��������</param>
		public Table(string name, string partitionName) : this(name, partitionName, new List<Column>())
		{
			this.partitionName = partitionName;
		}
		
		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="columns">�������</param>
		public Table(string name, List<Column> columns) : this(name, "PRIMARY", columns)
		{
		}

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="partitionName">��������</param>
		/// <param name="columns">�������</param>
		public Table(string name, string partitionName, List<Column> columns) : base(name)
		{
			this.columns = columns;
			this.partitionName = partitionName;
		}

		/// <summary>
		/// �������
		/// </summary>
		public List<Column> Columns
		{
			get
			{
				return columns;
			}
		}

		/// <summary>
		/// �������� ��������
		/// </summary>
		public string PartitionName
		{
			get { return partitionName; }
		}

		/// <summary>
		/// ��������� ������� � �������
		/// </summary>
		/// <param name="column">�������</param>
		/// <returns>�������, � ���������� ��������</returns>
		public Table AddColumn(Column column)
		{
			columns.Add(column);
			return this;
		}

		/// <summary>
		/// ��������� ��������� ����
		/// </summary>
		/// <param name="primaryKey">��������� ����</param>
		/// <returns>������� � ��������� ������</returns>
		public Table AddPrimaryKey(PrimaryKey primaryKey)
		{
			return this;
		}
	}
}
