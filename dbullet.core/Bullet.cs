//-----------------------------------------------------------------------
// <copyright file="Bullet.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using dbullet.core.dbo;
using dbullet.core.dbs;
using dbullet.core.engine;

namespace dbullet.core
{
	/// <summary>
	/// Операции, которые можно произвести над БД
	/// </summary>
	public abstract class Bullet : IDatabaseStrategy
	{
		/// <summary>
		/// Обновление
		/// </summary>
		public abstract void Update();

		/// <summary>
		/// Отмена обновления
		/// </summary>
		public abstract void Downgrade();

		/// <summary>
		/// Создаёт таблицу
		/// </summary>
		/// <param name="table">Таблица</param>
		public void CreateTable(Table table)
		{
			Executor.DatabaseStrategy.CreateTable(table);
		}

		/// <summary>
		/// Существует ли таблица
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		/// <returns>true - если существует, иначе false</returns>
		public bool IsTableExist(string tableName)
		{
			return Executor.DatabaseStrategy.IsTableExist(tableName);
		}

		/// <summary>
		/// Удаляет таблицу
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		public void DropTable(string tableName)
		{
			Executor.DatabaseStrategy.DropTable(tableName);
		}
	}
}
