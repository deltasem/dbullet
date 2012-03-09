//-----------------------------------------------------------------------
// <copyright file="SysStrategyBase.cs" company="SofTrust" author="SKiryshin">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Data;
using dbullet.core.dbo;
using dbullet.core.dbs;

namespace dbullet.core.engine.common
{
	/// <summary>
	/// Базовая системная стратегия
	/// </summary>
	public class SysStrategyBase : ISysDatabaseStrategy
	{
		/// <summary>
		/// Подключение к базе
		/// </summary>
		private IDbConnection connection;

		/// <summary>
		/// Стратегия работы с БД
		/// </summary>
		private IDatabaseStrategy strategy;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="connection">Соединение с БД</param>
		/// <param name="strategy">Стратегия работы с БД</param>
		public SysStrategyBase(IDbConnection connection, IDatabaseStrategy strategy)
		{
			this.connection = connection;
			this.strategy = strategy;
		}

		/// <summary>
		/// Инициализация базы данных
		/// Добавление, если нет системной таблицы
		/// </summary>
		/// <param name="name">Имя</param>
		public void InitDatabase(string name)
		{
			if (!strategy.IsTableExist("dbullet"))
			{
				strategy.CreateTable(new Table("dbullet").AddColumn(new Column("Version", DbType.Int32)));
			}

			if (!strategy.IsColumnExists("dbullet", "Assembly"))
			{
				var column = new Column("Assembly", DbType.String.Size(1024), false)
				             	{
				             		Constraint = new ValueDefault("dbullet_assembly_default", string.Format("'{0}'", name))
				             	};
				strategy.AddColumn(new Table("dbullet"), column);
			}
		}

		/// <summary>
		/// Возвращает последнюю версию базы
		/// </summary>
		/// <param name="name">Имя</param>
		/// <returns>Версия базы</returns>
		public int GetLastVersion(string name)
		{
			try
			{
				connection.Open();
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = string.Format("select max(Version) from dbullet where assembly = '{0}'", name);
					var res = cmd.ExecuteScalar();
					if (res == System.DBNull.Value)
					{
						return 0;
					}

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
		/// <param name="name">Имя</param>
		public void SetCurrentVersion(int version, string name)
		{
			try
			{
				connection.Open();
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = string.Format("insert into dbullet(Version, Assembly) values({0}, '{1}')", version, name);
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

		/// <summary>
		/// Удаление информации об указанной версии
		/// </summary>
		/// <param name="version">Версия</param>
		/// <param name="name">Имя</param>
		public void RemoveVersionInfo(int version, string name)
		{
			try
			{
				connection.Open();
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = string.Format("delete from dbullet where version = {0} and Assembly = '{1}'", version, name);
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