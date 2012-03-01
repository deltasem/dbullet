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
	/// ��������� ������ � ����� MS SQL 2008
	/// </summary>
	public class MsSql2008Strategy : StrategyBase, IDatabaseStrategy
	{
		/// <summary>
		/// ������
		/// </summary>
		private static readonly Logger log = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// �������� ����������
		/// </summary>
		private readonly MsSqlTemplateManager manager = new MsSqlTemplateManager();

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="connection">���������� � ��</param>
		public MsSql2008Strategy(IDbConnection connection) : base(connection)
		{
		}

		/// <summary>
		/// ������ �������
		/// </summary>
		/// <param name="table">�������</param>
		public void CreateTable(Table table)
		{
			if (table.Columns == null || table.Columns.Count == 0)
			{
				throw new ColumnExpectedException();
			}

			ExecuteNonQuery(Razor.Parse(manager.GetCreateTableTemplate(), table, "create table"));
			log.Info("������� {0} ������� � ������� {1}", table.Name, table.PartitionName);
		}

		/// <summary>
		/// ������� �������
		/// </summary>
		/// <param name="tableName">�������� �������</param>
		public void DropTable(string tableName)
		{
			if (string.IsNullOrEmpty(tableName))
			{
				throw new TableExpectedException();
			}

			ExecuteNonQuery(Razor.Parse(manager.GetDropTableTemplate(), new Table(tableName), "drop table"));
			log.Info("������� {0} �������", tableName);
		}

		/// <summary>
		/// ���������� �� �������
		/// </summary>
		/// <param name="tableName">�������� �������</param>
		/// <returns>true - ���� ����������, ����� false</returns>
		public bool IsTableExist(string tableName)
		{
			if (string.IsNullOrEmpty(tableName))
			{
				throw new ArgumentException();
			}

			return ExecuteScalar(Razor.Parse(manager.GetIsTableExistTemplate(), new Table(tableName), "table exists")).ToString() == "1";
		}

		/// <summary>
		/// ���������� �� �������� ������� � �������
		/// </summary>
		/// <param name="tableName">�������� �������</param>
		/// <param name="columnName">�������� �������</param>
		/// <returns>true - ���� ����������, ����� false</returns>
		public bool IsColumnExists(string tableName, string columnName)
		{
			if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(columnName))
			{
				throw new ArgumentException();
			}

			return ExecuteScalar(Razor.Parse(manager.GetIsColumnExistTemplate(), new object[] { tableName, columnName }, "column exists")).ToString() == "1";
		}

		/// <summary>
		/// ������ ������
		/// </summary>
		/// <param name="index">������</param>
		public void CreateIndex(Index index)
		{
			ExecuteNonQuery(Razor.Parse(manager.GetCreateIndexTemplate(), index, "create index"));
			log.Info("������ {0} ������ � ������� {1}", index.Name, index.PartitionName);
		}

		/// <summary>
		/// ������� ������
		/// </summary>
		/// <param name="index">������</param>
		public void DropIndex(Index index)
		{
			ExecuteNonQuery(Razor.Parse(manager.GetDropIndexTemplate(), index, "drop index"));
			log.Info("������ {0} ������", index.Name);
		}

		/// <summary>
		/// ��������� �������
		/// </summary>
		/// <param name="table">�������</param>
		/// <param name="column">�������</param>
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
			
			log.Info("�������� ������� {0} � ������� {1}", column.Name, table.Name);
		}

		/// <summary>
		/// ������� ������� ����
		/// </summary>
		/// <param name="foreignKey">������� ����</param>
		public void CreateForeignKey(ForeignKey foreignKey)
		{
			ExecuteNonQuery(Razor.Parse(manager.GetCreateForeignKeyTemplate(), foreignKey, "create foreignkey"));
			log.Info("������� ���� ������ {0}", foreignKey);
		}

		/// <summary>
		/// ������� ������� ����
		/// </summary>
		/// <param name="foreignKey">������� ����</param>
		public void DropForeignKey(ForeignKey foreignKey)
		{
			ExecuteNonQuery(Razor.Parse(manager.GetDropForeignKeyTemplate(), foreignKey, "drop foreignkey"));
			log.Info("������� ���� {0} ������", foreignKey.Name);
		}
		
		/// <summary>
		/// ��������� ������ � �������
		/// </summary>
		/// <param name="table">�������</param>
		/// <param name="identity">true - ��������� �������� ������������</param>
		/// <param name="rows">������ �������</param>
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
		/// ��������� ����� � ����. ������ � ������� CSV
		/// </summary>
		/// <param name="tableName">������� ��� ��������</param>
		/// <param name="stream">������� �����</param>
		/// <param name="modulator">��������������</param>
		/// <param name="csvQuotesType">��� ������� CSV</param>
		/// <param name="identity">true - ��������� �������� ������������</param>
		public void LoadCsv(string tableName, StreamReader stream, Dictionary<string, Func<string, object>> modulator, CsvQuotesType csvQuotesType = CsvQuotesType.DoubleQuotes, bool identity = false)
		{
			DateTime begin = DateTime.Now;
			try
			{
				log.Info("����� ������� ������ � ������� {0}", tableName);
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

				log.Info("������ �������� �� {0}", DateTime.Now - begin);
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
		/// ������� ������� �� �������
		/// </summary>
		/// <param name="table">�������</param>
		/// <param name="column">�������</param>
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
			log.Info("������� {0} ������� �� ������� {1}", column, table);
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
