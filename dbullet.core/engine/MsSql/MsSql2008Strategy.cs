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
using dbullet.core.engine.common;
using dbullet.core.exception;
using dbullet.core.tools;
using NLog;
using RazorEngine;
using RazorEngine.Templating;

namespace dbullet.core.engine.MsSql
{
	/// <summary>
	/// Стратегия работы с базой MS SQL 2008
	/// </summary>
	public class MsSql2008Strategy : StrategyBase, IDatabaseStrategy
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
		/// Конструктор
		/// </summary>
		/// <param name="connection">Соединение с БД</param>
		public MsSql2008Strategy(IDbConnection connection) : base(connection)
		{
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

			return ExecuteScalar(Razor.Parse(manager.GetIsColumnExistTemplate(), new object[] { tableName, columnName }, "column exists")).ToString() == "1";
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

				ExecuteScalar(Razor.Parse(manager.GetInsertRowsTemplate(), new object[] { table, props, values, identity }, "insert rows"));
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
						cmd.CommandText = string.Format("set identity_insert [{0}] on;", tableName);
						cmd.ExecuteNonQuery();
					}

					var headers = CsvParser.Parse(firstLine, csvQuotesType);
					cmd.CommandText = Razor.Parse(
						manager.GetInsertRowsStreamTemplate(), 
						new object[] { tableName, headers },
						"insert rows stream").Replace("\r", string.Empty).Replace("\n", string.Empty);
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

					if (identity)
					{
						cmd.CommandText = string.Format("set identity_insert [{0}] off;", tableName);
						cmd.ExecuteNonQuery();
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

			ExecuteNonQuery(Razor.Parse(manager.GetDropColumnTemplate(), new object[] { table, column }, "drop column"));
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
				ExecuteScalar(Razor.Parse(manager.GetDeleteRowsTemplate(), new object[] { table, null, null }, "delete rows"));
			}

			foreach (var row in eq)
			{
				var props = row.GetType().GetProperties();
				var values = new string[props.Length];
				for (int i = 0; i < props.Length; i++)
				{
					values[i] = props[i].GetValue(row, null).ToString();
				}

				ExecuteScalar(Razor.Parse(manager.GetDeleteRowsTemplate(), new object[] { table, props, values }, "delete rows"));
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
	}
}
