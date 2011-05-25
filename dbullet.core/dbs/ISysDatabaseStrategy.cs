//-----------------------------------------------------------------------
// <copyright file="ISysDatabaseStrategy.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbs
{
	/// <summary>
	/// Методы для системных манипуляций
	/// Увеличение / уменьшение версии, инициализация системной таблицы
	/// </summary>
	internal interface ISysDatabaseStrategy
	{
		/// <summary>
		/// Инициализация базы данных
		/// Добавление, если нет системной таблицы
		/// </summary>
		void InitDatabase();

		/// <summary>
		/// Возвращает последнюю версию базы
		/// </summary>
		/// <returns>Версия базы</returns>
		int GetLastVersion();

		/// <summary>
		/// Установка текущей версии
		/// </summary>
		/// <param name="version">Версия</param>
		void SetCurrentVersion(int version);
	}
}
