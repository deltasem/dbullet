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
		void SetCurrentVersion();
	}
}
