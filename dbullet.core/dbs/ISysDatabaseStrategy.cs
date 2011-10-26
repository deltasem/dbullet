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
		void InitDatabase();

		/// <summary>
		/// Возвращает последнюю версию базы
		/// </summary>
		/// <param name="assembly">Сборка с булетами</param>
		/// <returns>Версия базы</returns>
		int GetLastVersion(Assembly assembly);

		/// <summary>
		/// Установка текущей версии
		/// </summary>
		/// <param name="assembly">Сборка с булетами</param>
		/// <param name="version">Версия</param>
		void SetCurrentVersion(Assembly assembly, int version);

		/// <summary>
		/// Удаление информации об указанной версии
		/// </summary>
		/// <param name="version">Версия</param>
		void RemoveVersionInfo(int version);
	}
}
