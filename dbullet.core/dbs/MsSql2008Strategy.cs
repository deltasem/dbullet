namespace dbullet.core.dbs
{
	using System.Data.SqlClient;
	using dbullet.core.dbo;

	/// <summary>
	/// Стратегия работы с базой MS SQL 2008
	/// </summary>
	internal class MsSql2008Strategy : IDatabaseStrategy
	{
		/// <summary>
		/// Строка подключения
		/// </summary>
		private readonly string connectionString;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="connectionString">Строка подключения</param>
		public MsSql2008Strategy(string connectionString)
		{
			this.connectionString = connectionString;
		}

		/// <summary>
		/// Создаёт таблицу
		/// </summary>
		/// <param name="table">Таблица</param>
		public void CreateTable(Table table)
		{
			var connection = new SqlConnection(this.connectionString);
			connection.Open();
			using (var cmd = new SqlCommand(string.Empty, connection))
			{
				cmd.CommandText = string.Format("create table {0}", table.Name);
				cmd.ExecuteNonQuery();
			}

			connection.Close();
		}
	}
}
