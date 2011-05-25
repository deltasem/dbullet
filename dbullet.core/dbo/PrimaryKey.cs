//-----------------------------------------------------------------------
// <copyright file="PrimaryKey.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Первичный ключ
	/// </summary>
	public class PrimaryKey : DatabaseObjectBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название объекта</param>
		public PrimaryKey(string name) : base(name)
		{
		}
	}
}
