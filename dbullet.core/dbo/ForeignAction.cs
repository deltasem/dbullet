//-----------------------------------------------------------------------
// <copyright file="ForeignAction.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Действия при удалении
	/// </summary>
	public enum ForeignAction
	{
		/// <summary>
		/// Нет действия
		/// </summary>
		NoAction,
		
		/// <summary>
		/// Каскадное удаление
		/// </summary>
		Cascade,

		/// <summary>
		/// Установка null
		/// </summary>
		SetNull,
	}
}