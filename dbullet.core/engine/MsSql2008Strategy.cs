//-----------------------------------------------------------------------
// <copyright file="MsSql2008Strategy.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Data.SqlClient;
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
		private readonly SqlConnection connection;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="connection">Соединение с БД</param>
		public MsSql2008Strategy(SqlConnection connection)
		{
			this.connection = connection;
		}

		#region CreateTable
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

			try
			{
				connection.Open();
				using (var cmd = new SqlCommand(string.Empty, connection))
				{
					var t = Razor.Parse(manager.GetCreateTableTemplate(), table, "create table");
					cmd.CommandText = t;
					cmd.ExecuteNonQuery();
				}
			}
			catch (TemplateCompilationException ex)
			{
				ex.Errors.ToList().ForEach(p => Console.WriteLine(p.ErrorText));
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
		/// Удаляет таблицу
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		public void DropTable(string tableName)
		{
			if (string.IsNullOrEmpty(tableName))
			{
				throw new TableExpectedException();
			}

			try
			{
				connection.Open();
				using (var cmd = new SqlCommand(string.Empty, connection))
				{
					cmd.CommandText = Razor.Parse(manager.GetDropTableTemplate(), new Table(tableName), "drop table");
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

		#endregion

		#region IsTableExist
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

			try
			{
				connection.Open();
				var str = Razor.Parse(manager.GetIsTableExistTemplate(), new Table(tableName), "table exists");
				using (var cmd = new SqlCommand(str, connection))
				{
					return cmd.ExecuteScalar().ToString() == "1";
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
		/// Создаёт индекс
		/// </summary>
		/// <param name="index">Индеес</param>
		public void CreateIndex(Index index)
		{
			try
			{
				connection.Open();
				var t = Razor.Parse(manager.GetCreateIndexTemplate(), index, "create index");
				using (var cmd = new SqlCommand(t, connection))
				{
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

		#endregion
	}
}
