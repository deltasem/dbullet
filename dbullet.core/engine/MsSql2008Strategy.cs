//-----------------------------------------------------------------------
// <copyright file="MsSql2008Strategy.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using dbullet.core.dbo;
using dbullet.core.dbs;
using dbullet.core.engine.MsSql;
using dbullet.core.exception;
using dbullet.core.tools;
using RazorEngine;

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
		/// Удалить индекс
		/// </summary>
		/// <param name="index">Индекс</param>
		public void DropIndex(Index index)
		{
			ExecuteNonQuery(Razor.Parse(manager.GetDropIndexTemplate(), index, "drop index"));
		}

		/// <summary>
		/// Создать внешний ключ
		/// </summary>
		/// <param name="foreignKey">Внешний ключ</param>
		public void CreateForeignKey(ForeignKey foreignKey)
		{
			ExecuteNonQuery(Razor.Parse(manager.GetCreateForeignKeyTemplate(), foreignKey, "create foreignkey"));
		}

		/// <summary>
		/// Удалить внешний ключ
		/// </summary>
		/// <param name="foreignKey">Внешний ключ</param>
		public void DropForeignKey(ForeignKey foreignKey)
		{
			ExecuteNonQuery(Razor.Parse(manager.GetDropForeignKeyTemplate(), foreignKey, "drop foreignkey"));
		}

		/// <summary>
		/// Добавляет записи в таблицу
		/// </summary>
		/// <param name="table">Таблицы</param>
		/// <param name="rows">Записи</param>
		public void InsertRows(string table, params object[] rows)
		{
			if (string.IsNullOrEmpty(table))
			{
				throw new TableExpectedException();
			}

			if (rows == null || rows.Length == 0)
			{
				throw new ArgumentNullException();
			}

			foreach (var row in rows)
			{
				var props = row.GetType().GetProperties();
				var values = new string[props.Length];
				for (int i = 0; i < props.Length; i++)
				{
					values[i] = props[i].GetValue(row, null).ToString();
				}

				ExecuteScalar(Razor.Parse(manager.GetInsertRowsTemplate(), new object[] { table, props, values }, "insert rows"));
			}
		}

		/// <summary>
		/// Загружает поток в базу. Данные в формате CSV
		/// </summary>
		/// <param name="tableName">Таблица для загрузки</param>
		/// <param name="stream">Входной поток</param>
		/// <param name="modulator">Преобразования</param>
		/// <param name="csvQuotesType">Тип кавычек CSV</param>
		public void LoadCsv(string tableName, StreamReader stream, Dictionary<string, Func<string, string>> modulator, CsvQuotesType csvQuotesType = CsvQuotesType.DoubleQuotes)
		{
			try
			{
				var firstLine = stream.ReadLine();
				if (string.IsNullOrEmpty(firstLine))
				{
					throw new InvalidDataException();
				}
				
				connection.Open();
				using (IDbCommand cmd = connection.CreateCommand())
				{
					var headers = CsvParser.Parse(firstLine, csvQuotesType);
					cmd.CommandText = Razor.Parse(
						manager.GetInsertRowsStreamTemplate(), 
						new object[] { tableName, headers },
						"insert rows stream");
					IDbDataParameter[] dataParams = new IDbDataParameter[headers.Length];
					for (int i = 0; i < dataParams.Length; i++)
					{
						IDbDataParameter parameter = cmd.CreateParameter();
						parameter.ParameterName = headers[i];
						dataParams[i] = parameter;
					}

					for (var line = stream.ReadLine(); line != null; line = stream.ReadLine())
					{
						var lineValues = CsvParser.Parse(line, csvQuotesType);
						if (lineValues.Length != dataParams.Length)
						{
							throw new InvalidDataException();
						}

						for (int i = 0; i < lineValues.Length; i++)
						{
							dataParams[i].Value = lineValues[i];
						}

						cmd.ExecuteNonQuery();
					}
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
