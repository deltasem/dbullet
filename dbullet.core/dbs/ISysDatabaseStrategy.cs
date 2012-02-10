//-----------------------------------------------------------------------
// <copyright file="ISysDatabaseStrategy.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Reflection;

namespace dbullet.core.dbs
{
	/// <summary>
	/// Методы для системных манипуляций
	/// Увеличение / уменьшение версии, инициализация системной таблицы
	/// </summary>
	public interface ISysDatabaseStrategy
	{
		/// <summary>
		/// Инициализация базы данных
		/// Добавление, если нет системной таблицы
		/// </summary>
		/// <param name="name">Имя</param>
		void InitDatabase(string name);

		/// <summary>
		/// Возвращает последнюю версию базы
		/// </summary>
		/// <param name="name">Имя</param>
		/// <returns>Версия базы</returns>
		int GetLastVersion(string name);

		/// <summary>
		/// Установка текущей версии
		/// </summary>
		/// <param name="version">Версия</param>
		/// <param name="name">Имя</param>
		void SetCurrentVersion(int version, string name);

		/// <summary>
		/// Удаление информации об указанной версии
		/// </summary>
		/// <param name="version">Версия</param>
		/// <param name="name">Имя</param>
		void RemoveVersionInfo(int version, string name);
	}
}
