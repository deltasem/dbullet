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
	public class Table : DatabaseObjectBase
	{
		/// <summary>
		/// �������
		/// </summary>
		private readonly List<Column> columns;

		/// <summary>
		/// ��������
		/// </summary>
		private readonly Partition partition;

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="columns">�������</param>
		public Table(string name, List<Column> columns) : this(name, new Partition("PRIMARY"), columns)
		{
		}

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="partition">��������</param>
		/// <param name="columns">�������</param>
		public Table(string name, Partition partition, List<Column> columns) : base(name)
		{
			this.columns = columns;
			this.partition = partition;
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
		/// ��������
		/// </summary>
		public Partition Partition
		{
			get
			{
				return partition;
			}
		}
	}
}
