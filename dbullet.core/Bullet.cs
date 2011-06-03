//-----------------------------------------------------------------------
// <copyright file="Bullet.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
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
		/// Создаёт индекс
		/// </summary>
		/// <param name="index">Индеес</param>
		public void CreateIndex(Index index)
		{
			Executor.DatabaseStrategy.CreateIndex(index);
		}

		/// <summary>
		/// Удаляет таблицу
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		public void DropTable(string tableName)
		{
			Executor.DatabaseStrategy.DropTable(tableName);
		}

		/// <summary>
		/// Удаляет индекс
		/// </summary>
		/// <param name="index">Индеес</param>
		public void DropIndex(Index index)
		{
			Executor.DatabaseStrategy.DropIndex(index);
		}

		/// <summary>
		/// Создать внешний ключ
		/// </summary>
		/// <param name="foreignKey">Внешний ключ</param>
		public void CreateForeignKey(ForeignKey foreignKey)
		{
			Executor.DatabaseStrategy.CreateForeignKey(foreignKey);
		}

		/// <summary>
		/// Удалить внешний ключ
		/// </summary>
		/// <param name="foreignKey">Внешний ключ</param>
		public void DropForeignKey(ForeignKey foreignKey)
		{
			Executor.DatabaseStrategy.DropForeignKey(foreignKey);
		}

		/// <summary>
		/// Удалить внешний ключ
		/// </summary>
		/// <param name="name">Имя ключа</param>
		/// <param name="tableName">Таблица с ключем</param>
		public void DropForeignKey(string name, string tableName)
		{
			DropForeignKey(new ForeignKey(name, tableName, string.Empty, string.Empty, string.Empty));
		}

		/// <summary>
		/// Добавляет записи в таблицу
		/// </summary>
		/// <param name="table">Таблицы</param>
		/// <param name="rows">Записи</param>
		public void InsertRows(string table, params object[] rows)
		{
			Executor.DatabaseStrategy.InsertRows(table, rows);
		}
	}
}
