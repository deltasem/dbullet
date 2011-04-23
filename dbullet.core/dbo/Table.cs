//-----------------------------------------------------------------------
// <copyright file="Table.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Таблица
	/// </summary>
	public class Table : DatabaseObjectBase
	{
		/// <summary>
		/// Колонки
		/// </summary>
		private readonly Column[] columns;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название таблицы</param>
		/// <param name="columns">Столбцы</param>
		public Table(string name, params Column[] columns) : base(name)
		{
			this.columns = columns;
		}

		/// <summary>
		/// Столбцы
		/// </summary>
		public Column[] Columns
		{
			get
			{
				return this.columns;
			}
		}
	}
}