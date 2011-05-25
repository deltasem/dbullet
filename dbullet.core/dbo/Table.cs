//-----------------------------------------------------------------------
// <copyright file="Table.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	using System.Collections.Generic;

	/// <summary>
	/// Таблица
	/// </summary>
	public class Table : DatabaseObjectBase, IPartitionable
	{
		/// <summary>
		/// Колонки
		/// </summary>
		private readonly List<Column> columns;

		/// <summary>
		/// Название партиции
		/// </summary>
		private string partitionName;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название таблицы</param>
		/// <param name="columns">Столбцы</param>
		public Table(string name, List<Column> columns) : this(name, "PRIMARY", columns)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название таблицы</param>
		/// <param name="partitionName">Партиция</param>
		/// <param name="columns">Столбцы</param>
		public Table(string name, string partitionName, List<Column> columns) : base(name)
		{
			this.columns = columns;
			this.partitionName = partitionName;
		}

		/// <summary>
		/// Столбцы
		/// </summary>
		public List<Column> Columns
		{
			get
			{
				return columns;
			}
		}

		#region Implementation of IPartitionable
		/// <summary>
		/// Название партиции
		/// </summary>
		public string PartitionName
		{
			get { return partitionName; }
		}
		#endregion
	}
}
