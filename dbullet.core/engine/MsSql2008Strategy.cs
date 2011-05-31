//-----------------------------------------------------------------------
// <copyright file="MsSql2008Strategy.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Data;
using System.Linq;
using dbullet.core.dbo;
using dbullet.core.dbs;
using dbullet.core.engine.MsSql;
using dbullet.core.exception;
using RazorEngine;
using RazorEngine.Templating;

namespace dbullet.core.engine
{
	/// <summary>
	/// Стратегия работы с базой MS SQL 2008
	/// </summary>
	public class MsSql2008Strategy : IDatabaseStrategy
	{
		/// <summary>
		/// Менеджер темплейтов
		/// </summary>
		private readonly MsSqlTemplateManager manager = new MsSqlTemplateManager();

		/// <summary>
		/// Подключение к базе
		/// </summary>
		private readonly IDbConnection connection;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="connection">Соединение с БД</param>
		public MsSql2008Strategy(IDbConnection connection)
		{
			this.connection = connection;
		}

		/// <summary>
		/// Создаёт таблицу
		/// </summary>
		/// <param name="table">Таблица</param>
		public void CreateTable(Table table)
		{
			if (table.Columns == null || table.Columns.Count == 0)
			{
				throw new CollumnExpectedException();
			}

			ExecuteNonQuery(Razor.Parse(manager.GetCreateTableTemplate(), table, "create table"));
		}

		/// <summary>
		/// Удаляет таблицу
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		public void DropTable(string tableName)
		{
			if (string.IsNullOrEmpty(tableName))
			{
				throw new TableExpectedException();
			}

			ExecuteNonQuery(Razor.Parse(manager.GetDropTableTemplate(), new Table(tableName), "drop table"));
		}

		/// <summary>
		/// Существует ли таблица
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		/// <returns>true - если существует, иначе false</returns>
		public bool IsTableExist(string tableName)
		{
			if (string.IsNullOrEmpty(tableName))
			{
				throw new ArgumentException();
			}

			return ExecuteScalar(Razor.Parse(manager.GetIsTableExistTemplate(), new Table(tableName), "table exists")).ToString() == "1";
		}

		/// <summary>
		/// Создаёт индекс
		/// </summary>
		/// <param name="index">Индеес</param>
		public void CreateIndex(Index index)
		{
			ExecuteNonQuery(Razor.Parse(manager.GetCreateIndexTemplate(), index, "create index"));
		}

		/// <summary>
		/// Выполнить запрос
		/// </summary>
		/// <param name="commandText">запрос</param>
		/// <returns>Результат</returns>
		private object ExecuteScalar(string commandText)
		{
			try
			{
				connection.Open();
				using (IDbCommand cmd = connection.CreateCommand())
				{
					cmd.CommandText = commandText;
					return cmd.ExecuteScalar();
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
		/// Выполнить запрос
		/// </summary>
		/// <param name="commandText">Запрос</param>
		private void ExecuteNonQuery(string commandText)
		{
			try
			{
				connection.Open();
				using (IDbCommand cmd = connection.CreateCommand())
				{
					cmd.CommandText = commandText;
					cmd.ExecuteNonQuery();
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
