//-----------------------------------------------------------------------
// <copyright file="DatabaseObjectBase.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Базовый объект базы данных
	/// </summary>
	public abstract class DatabaseObjectBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название объекта</param>
		protected DatabaseObjectBase(string name)
		{
			this.Name = name;
		}

		/// <summary>
		/// Имя
		/// </summary>
		public string Name { get; set; }
	}
}
