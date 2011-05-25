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
	public class Table : PartitionableObject
	{
		/// <summary>
		/// Колонки
		/// </summary>
		private readonly List<Column> columns;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название таблицы</param>
		/// <param name="columns">Столбцы</param>
		public Table(string name, List<Column> columns) : this(name, new Partition("PRIMARY"), columns)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название таблицы</param>
		/// <param name="partition">Партишин</param>
		/// <param name="columns">Столбцы</param>
		public Table(string name, Partition partition, List<Column> columns) : base(name, partition)
		{
			this.columns = columns;
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название таблицы</param>
		/// <param name="partitionName">Партиция</param>
		/// <param name="columns">Столбцы</param>
		public Table(string name, string partitionName, List<Column> columns) : base(name, partitionName)
		{
			this.columns = columns;
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
	}
}
