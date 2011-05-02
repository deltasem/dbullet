using System;
using System.Data.SqlClient;

namespace dbullet.core.dbs
{
	/// <summary>
	/// Реализация длы MS SQL 2008 стратегии
	/// </summary>
	internal class MsSql2008SysStrategy : ISysDatabaseStrategy
	{
		/// <summary>
		/// Подключение к базе
		/// </summary>
		private readonly SqlConnection connection;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="connection">Соединение с БД</param>
		public MsSql2008SysStrategy(SqlConnection connection)
		{
			this.connection = connection;
		}

		/// <summary>
		/// Инициализация базы данных
		/// Добавление, если нет системной таблицы
		/// </summary>
		public void InitDatabase()
		{
		}

		/// <summary>
		/// Возвращает последнюю версию базы
		/// </summary>
		/// <returns>Версия базы</returns>
		public int GetLastVersion()
		{
			return 0;
		}

		/// <summary>
		/// Установка текущей версии
		/// </summary>
		public void SetCurrentVersion()
		{
		}
	}
}
