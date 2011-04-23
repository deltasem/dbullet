//-----------------------------------------------------------------------
// <copyright file="Column.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Столбец таблицы
	/// </summary>
	public class Column : DatabaseObjectBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название столбца</param>
		public Column(string name) : base(name)
		{
		}
	}
}