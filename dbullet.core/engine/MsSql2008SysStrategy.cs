//-----------------------------------------------------------------------
// <copyright file="MsSql2008SysStrategy.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using dbullet.core.dbo;
using dbullet.core.dbs;

namespace dbullet.core.engine
{
	/// <summary>
	/// Реализация длы MS SQL 2008 стратегии
	/// </summary>
	public class MsSql2008SysStrategy : ISysDatabaseStrategy
	{
		/// <summary>
		/// Подключение к базе
		/// </summary>
		private readonly SqlConnection connection;

		/// <summary>
		/// Стратегия работы с базой
		/// </summary>
		private readonly MsSql2008Strategy sqlStrategy;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="connection">Соединение с БД</param>
		public MsSql2008SysStrategy(SqlConnection connection)
		{
			this.connection = connection;
			sqlStrategy = new MsSql2008Strategy(this.connection);
		}

		/// <summary>
		/// Инициализация базы данных
		/// Добавление, если нет системной таблицы
		/// </summary>
		public void InitDatabase()
		{
			if (!sqlStrategy.IsTableExist("dbullet"))
			{
				// todo: возможно стоит сделать привязку версии к сборке, тогда можно будет сделать "независимые" тематики
				// todo: возможно стоит добавить дату проведения обновления
				sqlStrategy.CreateTable(new Table("dbullet").AddColumn(new Column("Version", DbType.Int32)));
			}
		}

		/// <summary>
		/// Возвращает последнюю версию базы
		/// </summary>
		/// <returns>Версия базы</returns>
		public int GetLastVersion()
		{
			try
			{
				connection.Open();
				using (var cmd = new SqlCommand("select max(Version) from dbullet", connection))
				{
					return int.Parse(cmd.ExecuteScalar().ToString());
				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
		}

		/// <summary>
		/// Установка текущей версии
		/// </summary>
		/// <param name="version">Версия</param>
		public void SetCurrentVersion(int version)
		{
			try
			{
				connection.Open();
				using (var cmd = new SqlCommand(string.Empty, connection))
				{
					cmd.CommandText = string.Format("insert into dbullet({0})", version);
					cmd.ExecuteScalar();
				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
		}
	}
}
