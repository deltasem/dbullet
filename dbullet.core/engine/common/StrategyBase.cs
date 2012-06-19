//-----------------------------------------------------------------------
// <copyright file="StrategyBase.cs" company="SofTrust" author="SKiryshin">
//     Copyright (c) 2012. All rights reserved.
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
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace dbullet.core.engine.common
{
	/// <summary>
	/// Базовая стратегия
	/// </summary>
	public abstract class StrategyBase : IDatabaseStrategy
	{
		/// <summary>
		/// Менеджер темплейтов
		/// </summary>
		protected readonly ITemplateManager TemplateManager = new MsSqlTemplateManager();

		/// <summary>
		/// Логгер
		/// </summary>
		private static readonly Logger log = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Рендер
		/// </summary>
		private static readonly TemplateService razor;

		/// <summary>
		/// Статический 
		/// </summary>
		static StrategyBase()
		{
			var cfg = new FluentTemplateServiceConfiguration(x => x.WithEncoding(Encoding.Raw));
			razor = new TemplateService(cfg);
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="connection">Соединение с БД</param>
		/// <param name="templateManager">Менеджер</param>
		public StrategyBase(IDbConnection connection, ITemplateManager templateManager)
		{
			TemplateManager = templateManager;
			this.connection = connection;
		}

		/// <summary>
		/// Стратегия
		/// </summary>
		public abstract SupportedStrategy Strategy { get; }

		/// <summary>
		/// Подключение к базе
		/// </summary>
		protected IDbConnection connection { get; private set; }

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

			ExecuteNonQuery(razor.Parse(TemplateManager.GetCreateTableTemplate(), table, "create table"));
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

			ExecuteNonQuery(razor.Parse(TemplateManager.GetDropTableTemplate(), new Table(tableName), "drop table"));
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

			return ExecuteScalar(razor.Parse(TemplateManager.GetIsTableExistTemplate(), new Table(tableName), "table exists")).ToString() == "1";
		}

		/// <summary>
		/// Существует ли заданная колонка в таблице
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		/// <param name="columnName">Название колонки</param>
		/// <returns>true - если существует, иначе false</returns>
		public bool IsColumnExists(string tableName, string columnName)
		{
			if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(columnName))
			{
				throw new ArgumentException();
			}

			return ExecuteScalar(razor.Parse(TemplateManager.GetIsColumnExistTemplate(), new object[] { tableName, columnName }, "column exists")).ToString() == "1";
		}

		/// <summary>
		/// Создаёт индекс
		/// </summary>
		/// <param name="index">Индеес</param>
		public void CreateIndex(Index index)
		{
			ExecuteNonQuery(razor.Parse(TemplateManager.GetCreateIndexTemplate(), index, "create index"));
			log.Info("Индекс {0} создан в разделе {1}", index.Name, index.PartitionName);
		}

		/// <summary>
		/// Удалить индекс
		/// </summary>
		/// <param name="index">Индекс</param>
		public void DropIndex(Index index)
		{
			ExecuteNonQuery(razor.Parse(TemplateManager.GetDropIndexTemplate(), index, "drop index"));
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
				ExecuteNonQuery(razor.Parse(TemplateManager.GetAddColumnTemplate(), new object[] { table, column }, "add column"));
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
			ExecuteNonQuery(razor.Parse(TemplateManager.GetCreateForeignKeyTemplate(), foreignKey, "create foreignkey"));
			log.Info("Внешний ключ создан {0}", foreignKey);
		}

		/// <summary>
		/// Удалить внешний ключ
		/// </summary>
		/// <param name="foreignKey">Внешний ключ</param>
		public void DropForeignKey(ForeignKey foreignKey)
		{
			ExecuteNonQuery(razor.Parse(TemplateManager.GetDropForeignKeyTemplate(), foreignKey, "drop foreignkey"));
			log.Info("Внешний ключ {0} удален", foreignKey.Name);
		}

		/// <summary>
		/// Добавляет записи в таблицу
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="identity">true - отключать идентити спецификацию</param>
		/// <param name="rows">Список записей</param>
		public void InsertRows(string table, bool identity = false, params object[] rows)
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

				ExecuteScalar(razor.Parse(TemplateManager.GetInsertRowsTemplate(), new object[] { table, props, values, identity }, "insert rows"));
			}
		}

		/// <summary>
		/// Загружает поток в базу. Данные в формате CSV
		/// </summary>
		/// <param name="tableName">Таблица для загрузки</param>
		/// <param name="stream">Входной поток</param>
		/// <param name="modulator">Преобразования</param>
		/// <param name="csvQuotesType">Тип кавычек CSV</param>
		/// <param name="identity">true - отключать идентити спецификацию</param>
		public void LoadCsv(string tableName, StreamReader stream, Dictionary<string, Func<string, object>> modulator, CsvQuotesType csvQuotesType = CsvQuotesType.DoubleQuotes, bool identity = false)
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
					if (identity)
					{
						EnableIdentityInsert(tableName, cmd);
					}

					var headers = CsvParser.Parse(firstLine, csvQuotesType);
					cmd.CommandText = razor.Parse(
						TemplateManager.GetInsertRowsStreamTemplate(), 
						new object[] { tableName, headers },
						"insert rows stream").Replace("\r", string.Empty).Replace("\n", string.Empty);
					var dataParams = new IDbDataParameter[headers.Length];
					for (int i = 0; i < dataParams.Length; i++)
					{
						IDbDataParameter parameter = cmd.CreateParameter();
						parameter.ParameterName = GetParameterName(headers, i);
						parameter.DbType = DbType.String;
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

					if (identity)
					{
						DisableIdentityInsert(tableName, cmd);
					}
				}

				log.Info("Импорт завершен за {0}", DateTime.Now - begin);
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

			ExecuteNonQuery(razor.Parse(TemplateManager.GetDropColumnTemplate(), new object[] { table, column }, "drop column"));
			log.Info("Колонка {0} удалена из таблицы {1}", column, table);
		}

		/// <summary>
		/// Remove rows from table
		/// </summary>
		/// <param name="table">Table</param>
		/// <param name="eq">Equality conditions</param>
		/// <example>DeleteRows("sometable", new { ID = 1 }, new { ID = 2 })</example>
		public void DeleteRows(string table, params object[] eq)
		{
			if (eq == null || eq.Length == 0)
			{
				ExecuteScalar(razor.Parse(TemplateManager.GetDeleteRowsTemplate(), new object[] { table, null, null }, "delete rows"));
			}

			foreach (var row in eq)
			{
				var props = row.GetType().GetProperties();
				var values = new string[props.Length];
				for (int i = 0; i < props.Length; i++)
				{
					values[i] = props[i].GetValue(row, null).ToString();
				}

				ExecuteScalar(razor.Parse(TemplateManager.GetDeleteRowsTemplate(), new object[] { table, props, values }, "delete rows"));
			}
		}

		/// <summary>
		/// Remove from the table each row, using the equality condition
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="stream">Stream with CSV data</param>
		/// <param name="keyColumn">Equality column in CSV file</param>
		/// <param name="modulator">Converting keyColumn value to some types</param>
		/// <param name="csvQuotesType">Quotes type</param>
		/// <example>UnloadCsv("someTable", stream, "ID", x => (int)x)</example>
		public void UnloadCsv(string table, StreamReader stream, string keyColumn, Func<string, object> modulator = null, CsvQuotesType csvQuotesType = CsvQuotesType.DoubleQuotes)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Выполняет скрипт
		/// </summary>
		/// <param name="strategy">Стратегия</param>
		/// <param name="query">Запрос</param>
		public void ExecuteQuery(SupportedStrategy strategy, string query)
		{
			if (strategy == Strategy || strategy == SupportedStrategy.Any)
			{
				ExecuteNonQuery(query);
				log.Info("Выполнен пользовательский скрипт");
				log.Info(query);
			}
			else
			{
				log.Info("Пропущен пользовательский скрипт");
			}
		}

		/// <summary>
		/// identity_insert off
		/// </summary>
		/// <param name="tableName">Имя таблицы</param>
		/// <param name="cmd">Комманда</param>
		protected abstract void DisableIdentityInsert(string tableName, IDbCommand cmd);

		/// <summary>
		/// Возвращает имя параметра
		/// </summary>
		/// <param name="headers">Заголовки</param>
		/// <param name="i">ИД параметра</param>
		/// <returns>Имя параметра</returns>
		protected abstract string GetParameterName(string[] headers, int i);

		/// <summary>
		/// identity_insert on
		/// </summary>
		/// <param name="tableName">Имя таблицы</param>
		/// <param name="cmd">Комманда</param>
		protected abstract void EnableIdentityInsert(string tableName, IDbCommand cmd);

		/// <summary>
		/// Выполнить запрос
		/// </summary>
		/// <param name="commandText">запрос</param>
		/// <returns>Результат</returns>
		protected object ExecuteScalar(string commandText)
		{
			try
			{
				connection.Open();
				using (IDbCommand cmd = connection.CreateCommand())
				{
					cmd.CommandTimeout = 0;
					cmd.CommandText = commandText.Replace("\r", string.Empty).Replace("\n", string.Empty);
					return cmd.ExecuteScalar();
				}
			}
			catch (Exception ex)
			{
				log.Error(commandText);
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
		protected void ExecuteNonQuery(string commandText)
		{
			try
			{
				connection.Open();
				using (IDbCommand cmd = connection.CreateCommand())
				{
					cmd.CommandTimeout = 0;
					cmd.CommandText = commandText.Replace("\r", string.Empty).Replace("\n", string.Empty);
					cmd.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				log.Error(commandText);
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