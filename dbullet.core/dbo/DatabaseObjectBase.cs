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
		/// Имя
		/// </summary>
		private string name;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название объекта</param>
		protected DatabaseObjectBase(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// Имя
		/// </summary>
		public string Name
		{
			get
			{
				return this.name;
			}
		}
	}
}
