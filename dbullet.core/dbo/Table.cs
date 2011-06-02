//-----------------------------------------------------------------------
// <copyright file="Table.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Data;
using System.Linq;
using dbullet.core.exception;

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
		private Table(string name, List<Column> columns) : this(name, "PRIMARY", columns)
		{
		}

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="partitionName">��������</param>
		/// <param name="columns">�������</param>
		private Table(string name, string partitionName, List<Column> columns) : base(name)
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
		/// ��������� ������� � �������
		/// </summary>
		/// <param name="name">��� �������</param>
		/// <param name="dbType">DB ��� �������</param>
		/// <param name="nullable">����� ��������� null</param>
		/// <returns>�������, � ���������� ��������</returns>
		public Table AddColumn(string name, DbType dbType, bool nullable = true)
		{
			columns.Add(new Column(name, dbType, nullable));
			return this;
		}

		/// <summary>
		/// ��������� ������� � �������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="columnType">��� �������</param>
		/// <param name="nullable">����� ��������� null</param>
		/// <returns>�������, � ���������� ��������</returns>
		public Table AddColumn(string name, ColumnType columnType, bool nullable = true)
		{
			columns.Add(new Column(name, columnType, nullable));
			return this;
		}

		/// <summary>
		/// ��������� ��������� ����
		/// </summary>
		/// <param name="columnName">�������</param>
		/// <param name="partition">��������</param>
		/// <returns>������� � ��������� ������</returns>
		public Table AddPrimaryKey(string columnName, string partition = "PRIMARY")
		{
			var column = columns.FirstOrDefault(p => p.Name == columnName);
			if (column == null)
			{
				throw new CollumnExpectedException();
			}

			column.Constraint = new PrimaryKey(string.Format("PK_{0}", Name).ToUpper(), partition);
			return this;
		}

		/// <summary>
		/// ��������� �������� ��� ���������� �������
		/// </summary>
		/// <param name="defaultValue">��������� ��������</param>
		/// <returns>������� �������</returns>
		public Table Default(string defaultValue)
		{
			if (columns.Count == 0)
			{
				throw new CollumnExpectedException();
			}

			var column = columns.Last();
			if (column.Constraint != null)
			{
				throw new ConflictingDataException("������� �������� ������ ��� ����");
			}

			column.Constraint = new Default(string.Format("DF_{0}_{1}", Name, column.Name).ToUpper(), defaultValue);
			return this;
		}
	}
}
