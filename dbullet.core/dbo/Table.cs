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
	public class Table : DatabaseObjectBase
	{
		/// <summary>
		/// Колонки
		/// </summary>
		private readonly List<Column> columns;

		/// <summary>
		/// Партишин
		/// </summary>
		private readonly Partition partition;

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
		public Table(string name, Partition partition, List<Column> columns) : base(name)
		{
			this.columns = columns;
			this.partition = partition;
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

		/// <summary>
		/// Партишин
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
