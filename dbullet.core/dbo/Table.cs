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
	public class Table : PartitionableObject
	{
		/// <summary>
		/// �������
		/// </summary>
		private readonly List<Column> columns;

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
		public Table(string name, Partition partition, List<Column> columns) : base(name, partition)
		{
			this.columns = columns;
		}

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="partitionName">��������</param>
		/// <param name="columns">�������</param>
		public Table(string name, string partitionName, List<Column> columns) : base(name, partitionName)
		{
			this.columns = columns;
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
	}
}
