//-----------------------------------------------------------------------
// <copyright file="IndexColumn.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Колонка для добавления в индекс
	/// </summary>
	public class IndexColumn : DatabaseObjectBase
	{
		/// <summary>
		/// Направление сортировки
		/// </summary>
		private readonly Direction direction;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название объекта</param>
		/// <param name="direction">Направление сортировки</param>
		public IndexColumn(string name, Direction direction = Direction.Ascending) : base(name)
		{
			this.direction = direction;
		}

		/// <summary>
		/// Направление сортировки
		/// </summary>
		public Direction Direction
		{
			get { return direction; }
		}
	}
}