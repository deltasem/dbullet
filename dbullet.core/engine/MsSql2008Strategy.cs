//-----------------------------------------------------------------------
// <copyright file="MsSql2008Strategy.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using dbullet.core.dbo;
using dbullet.core.dbs;
using dbullet.core.engine.MsSql;
using dbullet.core.exception;
using dbullet.core.tools;
using NLog;
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
		/// Логгер
		/// </summary>
		private static readonly Logger log = LogManager.GetCurrentClassLogger();

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
				throw new ColumnExpectedException();
			}

			ExecuteNonQuery(Razor.Parse(manager.GetCreateTableTemplate(), table, "create table"));
			log.Info("Таблица {0} создана в разделе {1}", table.Name, table.PartitionName);
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
			log.Info("Таблица {0} удалена", tableName);
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
			log.Info("Индекс {0} создан в разделе {1}", index.Name, index.PartitionName);
		}

		/// <summary>
		/// Удалить индекс
		/// </summary>
		/// <param name="index">Индекс</param>
		public void DropIndex(Index index)
		{
			ExecuteNonQuery(Razor.Parse(manager.GetDropIndexTemplate(), index, "drop index"));
			log.Info("Индекс {0} удален", index.Name);
		}

		/// <summary>
		/// Добавляет колонку
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="column">Колонка</param>
		public void AddColumn(Table table, Column column)
		{
			if (!column.Nullable && (column.Constraint == null || !(column.Constraint is Default)))
			{
				throw new ArgumentException();
			}

			try
			{
				ExecuteNonQuery(Razor.Parse(manager.GetAddColumnTemplate(), new object[] { table, column }, "add column"));
			}
			catch (TemplateCompilationException ex)
			{
				ex.Errors.ToList().ForEach(p => Console.WriteLine(p.ErrorText));
			}
			
			log.Info("Добавлен столбец {0} в таблицу {1}", column.Name, table.Name);
		}

		/// <summary>
		/// Создать внешний ключ
		/// </summary>
		/// <param name="foreignKey">Внешний ключ</param>
		public void CreateForeignKey(ForeignKey foreignKey)
		{
			ExecuteNonQuery(Razor.Parse(manager.GetCreateForeignKeyTemplate(), foreignKey, "create foreignkey"));
			log.Info("Внешний ключ создан {0}", foreignKey);
		}

		/// <summary>
		/// Удалить внешний ключ
		/// </summary>
		/// <param name="foreignKey">Внешний ключ</param>
		public void DropForeignKey(ForeignKey foreignKey)
		{
			ExecuteNonQuery(Razor.Parse(manager.GetDropForeignKeyTemplate(), foreignKey, "drop foreignkey"));
			log.Info("Внешний ключ {0} удален", foreignKey.Name);
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
		public void LoadCsv(string tableName, StreamReader stream, Dictionary<string, Func<string, object>> modulator, CsvQuotesType csvQuotesType = CsvQuotesType.DoubleQuotes)
		{
			DateTime begin = DateTime.Now;
			try
			{
				log.Info("Страт импорта данных в таблицу {0}", tableName);
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
						parameter.ParameterName = string.Format("@{0}", headers[i]);
						dataParams[i] = parameter;
						cmd.Parameters.Add(parameter);
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
							if (modulator != null && modulator.ContainsKey(headers[i]))
							{
								dataParams[i].Value = modulator[headers[i]](lineValues[i]);
							}
							else
							{
								dataParams[i].Value = lineValues[i];
							}
						}

						cmd.ExecuteNonQuery();
					}
				}

				log.Info("Импорт завершен за {0}", DateTime.Now - begin);
			}
			catch (Exception ex)
			{
				log.Error(ex);
				throw;
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
		/// Удаляет колонку из таблицы
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="column">Колонка</param>
		public void DropColumn(string table, string column)
		{
			if (string.IsNullOrWhiteSpace(table))
			{
				throw new TableExpectedException();
			}

			if (string.IsNullOrWhiteSpace(column))
			{
				throw new ColumnExpectedException();
			}

			ExecuteNonQuery(Razor.Parse(manager.GetDropColumnTemplate(), new object[] { table, column }, "drop column"));
			log.Info("Колонка {0} удалена из таблицы {1}", column, table);
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
					cmd.CommandTimeout = 0;
					cmd.CommandText = commandText;
					return cmd.ExecuteScalar();
				}
			}
			catch (Exception ex)
			{
				log.Error(ex);
				throw;
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
					cmd.CommandTimeout = 0;
					cmd.CommandText = commandText;
					cmd.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				log.Error(ex);
				throw;
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
