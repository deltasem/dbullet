//-----------------------------------------------------------------------
// <copyright file="MsSql2008SysStrategy.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Data;
using System.Reflection;
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
		private readonly IDbConnection connection;

		/// <summary>
		/// Стратегия работы с БД
		/// </summary>
		private readonly IDatabaseStrategy strategy;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="connection">Соединение с БД</param>
		/// <param name="strategy">Стратегия работы с БД</param>
		public MsSql2008SysStrategy(IDbConnection connection, IDatabaseStrategy strategy)
		{
			this.connection = connection;
			this.strategy = strategy;
		}

		/// <summary>
		/// Инициализация базы данных
		/// Добавление, если нет системной таблицы
		/// </summary>
		/// <param name="assembly">Сборка с булетами</param>
		public void InitDatabase(Assembly assembly)
		{
			if (!strategy.IsTableExist("dbullet"))
			{
				strategy.CreateTable(new Table("dbullet").AddColumn(new Column("Version", DbType.Int32)));
			}

			if (!strategy.IsColumnExists("dbullet", "Assembly"))
			{
				var column = new Column("Assembly", DbType.String.Size(1024))
				             	{
				             		Constraint = new ValueDefault("dbullet_assembly_default", assembly.GetName().Name)
				             	};
				strategy.AddColumn(new Table("dbullet"), column);
			}
		}

		/// <summary>
		/// Возвращает последнюю версию базы
		/// </summary>
		/// <param name="assembly">Сборка с булетами</param>
		/// <returns>Версия базы</returns>
		public int GetLastVersion(Assembly assembly)
		{
			try
			{
				connection.Open();
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = string.Format("select max(Version) from dbullet where assembly = '{0}'", assembly.GetName().Name);
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
		/// <param name="assembly">Сборка с булетами</param>
		/// <param name="version">Версия</param>
		public void SetCurrentVersion(Assembly assembly, int version)
		{
			try
			{
				connection.Open();
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = string.Format("insert into dbullet(Version, Assembly) values({0}, '{1}')", version, assembly.GetName().Name);
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
		/// <param name="assembly">Сборка с булетами</param>
		/// <param name="version">Версия</param>
		public void RemoveVersionInfo(Assembly assembly, int version)
		{
			try
			{
				connection.Open();
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = string.Format("delete from dbullet where version = {0} and Assembly = '{1}'", version, assembly.GetName().Name);
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
