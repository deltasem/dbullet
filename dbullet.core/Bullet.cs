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
		/// Стратегия
		/// </summary>
		private readonly IDatabaseStrategy strategy = new MsSql2008Strategy(null);

		/// <summary>
		/// Версия
		/// </summary>
		public abstract int Version { get; }

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
			strategy.CreateTable(table);
		}

		/// <summary>
		/// Существует ли таблица
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		/// <returns>true - если существует, иначе false</returns>
		public bool IsTableExist(string tableName)
		{
			return strategy.IsTableExist(tableName);
		}
	}
}
