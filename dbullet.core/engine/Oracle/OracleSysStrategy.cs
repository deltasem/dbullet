//-----------------------------------------------------------------------
// <copyright file="OracleSysStrategy.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbs;

namespace dbullet.core.engine.Oracle
{
	/// <summary>
	/// Оракловая системная стратегия
	/// </summary>
	public class OracleSysStrategy : ISysDatabaseStrategy
	{
		/// <summary>
		/// Инициализация базы данных
		/// Добавление, если нет системной таблицы
		/// </summary>
		/// <param name="name">Имя</param>
		public void InitDatabase(string name)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Возвращает последнюю версию базы
		/// </summary>
		/// <param name="name">Имя</param>
		/// <returns>Версия базы</returns>
		public int GetLastVersion(string name)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Установка текущей версии
		/// </summary>
		/// <param name="version">Версия</param>
		/// <param name="name">Имя</param>
		public void SetCurrentVersion(int version, string name)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Удаление информации об указанной версии
		/// </summary>
		/// <param name="version">Версия</param>
		/// <param name="name">Имя</param>
		public void RemoveVersionInfo(int version, string name)
		{
			throw new System.NotImplementedException();
		}
	}
}