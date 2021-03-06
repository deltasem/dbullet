//-----------------------------------------------------------------------
// <copyright file="Default.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Дефалт для поля
	/// </summary>
	public abstract class Default : Constraint
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название дефалта</param>
		public Default(string name) : base(name)
		{
		}
	}
}