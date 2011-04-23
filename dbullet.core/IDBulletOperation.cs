//-----------------------------------------------------------------------
// <copyright file="IDBulletOperation.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core
{
	/// <summary>
	/// Операции, которые можно произвести над БД
	/// </summary>
	public interface IDBulletOperation
	{
		/// <summary>
		/// Добавление таблицы
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		void CreateTable(string tableName);
	}
}