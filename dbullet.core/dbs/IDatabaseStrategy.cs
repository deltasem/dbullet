//-----------------------------------------------------------------------
// <copyright file="IDatabaseStrategy.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using dbullet.core.tools;

namespace dbullet.core.dbs
{
	using dbo;

	/// <summary>
	/// Элементарные операции для работы с базой
	/// </summary>
	public interface IDatabaseStrategy
	{
		/// <summary>
		/// Добавляет колонку
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="column">Колонка</param>
		void AddColumn(Table table, Column column);

		/// <summary>
		/// Создать внешний ключ
		/// </summary>
		/// <param name="foreignKey">Внешний ключ</param>
		void CreateForeignKey(ForeignKey foreignKey);

		/// <summary>
		/// Создаёт индекс
		/// </summary>
		/// <param name="index">Индеес</param>
		void CreateIndex(Index index);

		/// <summary>
		/// Создаёт таблицу
		/// </summary>
		/// <param name="table">Таблица</param>
		void CreateTable(Table table);

		/// <summary>
		/// Удалить внешний ключ
		/// </summary>
		/// <param name="foreignKey">Внешний ключ</param>
		void DropForeignKey(ForeignKey foreignKey);

		/// <summary>
		/// Удаляет индекс
		/// </summary>
		/// <param name="index">Индеес</param>
		void DropIndex(Index index);

		/// <summary>
		/// Удаляет таблицу
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		void DropTable(string tableName);

		/// <summary>
		/// Добавляет записи в таблицу
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="identity">true - отключать идентити спецификацию</param>
		/// <param name="rows">Список записей</param>
		void InsertRows(string table, bool identity = false, params object[] rows);

		/// <summary>
		/// Существует ли таблица
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		/// <returns>true - если существует, иначе false</returns>
		bool IsTableExist(string tableName);

		/// <summary>
		/// Существует ли заданная колонка в таблице
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		/// <param name="columnName">Название колонки</param>
		/// <returns>true - если существует, иначе false</returns>
		bool IsColumnExists(string tableName, string columnName);

		/// <summary>
		/// Загружает поток в базу. Данные в формате CSV
		/// </summary>
		/// <param name="tableName">Таблица для загрузки</param>
		/// <param name="stream">Входной поток</param>
		/// <param name="modulator">Преобразования</param>
		/// <param name="csvQuotesType">Тип кавычек CSV</param>
		/// <param name="identity">true - отключать идентити спецификацию</param>
		void LoadCsv(string tableName, StreamReader stream, Dictionary<string, Func<string, object>> modulator, CsvQuotesType csvQuotesType = CsvQuotesType.DoubleQuotes, bool identity = false);

		/// <summary>
		/// Удаляет колонку из таблицы
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="column">Колонка</param>
		void DropColumn(string table, string column);
	}
}
