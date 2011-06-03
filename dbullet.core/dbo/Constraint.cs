//-----------------------------------------------------------------------
// <copyright file="Constraint.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Ограничение
	/// </summary>
	public class Constraint : DatabaseObjectBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название объекта</param>
		public Constraint(string name) : base(name)
		{
		}
	}
}
