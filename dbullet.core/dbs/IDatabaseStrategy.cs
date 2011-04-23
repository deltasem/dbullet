namespace dbullet.core.dbs
{
	using dbullet.core.dbo;

	/// <summary>
	/// Элементарные операции для работы с базой
	/// </summary>
	public interface IDatabaseStrategy
	{
		/// <summary>
		/// Создаёт таблицу
		/// </summary>
		/// <param name="table">Таблица</param>
		void CreateTable(Table table);
	}
}