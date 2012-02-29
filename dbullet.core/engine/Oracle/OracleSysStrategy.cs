//-----------------------------------------------------------------------
// <copyright file="OracleSysStrategy.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Data;
using dbullet.core.dbo;
using dbullet.core.dbs;

namespace dbullet.core.engine.Oracle
{
	/// <summary>
	/// Оракловая системная стратегия
	/// </summary>
	public class OracleSysStrategy : ISysDatabaseStrategy
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
		public OracleSysStrategy(IDbConnection connection, IDatabaseStrategy strategy)
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
			if (!this.strategy.IsTableExist("dbullet"))
			{
				this.strategy.CreateTable(new Table("dbullet").AddColumn(new Column("Version", DbType.Int32)));
			}

			if (!this.strategy.IsColumnExists("dbullet", "Assembly"))
			{
				var column = new Column("Assembly", DbType.String.Size(1024), false)
				{
					Constraint = new ValueDefault("dbullet_assembly_default", name)
				};
				this.strategy.AddColumn(new Table("dbullet"), column);
			}
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