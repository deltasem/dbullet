namespace dbullet.core.dbo
{
	/// <summary>
	/// База данных
	/// </summary>
	public class Database : DatabaseObjectBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Имя базы</param>
		public Database(string name) : base(name)
		{
		}

		/// <summary>
		/// Создёт таблицу
		/// </summary>
		/// <param name="table">Таблица</param>
		public void CreateTable(Table table)
		{
		}
	}
}
