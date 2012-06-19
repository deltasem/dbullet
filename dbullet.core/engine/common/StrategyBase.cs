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
	/// ������� ���������
	/// </summary>
	public abstract class StrategyBase : IDatabaseStrategy
	{
		/// <summary>
		/// �������� ����������
		/// </summary>
		protected readonly ITemplateManager TemplateManager = new MsSqlTemplateManager();

		/// <summary>
		/// ������
		/// </summary>
		private static readonly Logger log = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// ������
		/// </summary>
		private static readonly TemplateService razor;

		/// <summary>
		/// ����������� 
		/// </summary>
		static StrategyBase()
		{
			var cfg = new FluentTemplateServiceConfiguration(x => x.WithEncoding(Encoding.Raw));
			razor = new TemplateService(cfg);
		}

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="connection">���������� � ��</param>
		/// <param name="templateManager">��������</param>
		public StrategyBase(IDbConnection connection, ITemplateManager templateManager)
		{
			TemplateManager = templateManager;
			this.connection = connection;
		}

		/// <summary>
		/// ���������
		/// </summary>
		public abstract SupportedStrategy Strategy { get; }

		/// <summary>
		/// ����������� � ����
		/// </summary>
		protected IDbConnection connection { get; private set; }

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

			ExecuteNonQuery(razor.Parse(TemplateManager.GetCreateTableTemplate(), table, "create table"));
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

			ExecuteNonQuery(razor.Parse(TemplateManager.GetDropTableTemplate(), new Table(tableName), "drop table"));
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

			return ExecuteScalar(razor.Parse(TemplateManager.GetIsTableExistTemplate(), new Table(tableName), "table exists")).ToString() == "1";
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

			return ExecuteScalar(razor.Parse(TemplateManager.GetIsColumnExistTemplate(), new object[] { tableName, columnName }, "column exists")).ToString() == "1";
		}

		/// <summary>
		/// ������ ������
		/// </summary>
		/// <param name="index">������</param>
		public void CreateIndex(Index index)
		{
			ExecuteNonQuery(razor.Parse(TemplateManager.GetCreateIndexTemplate(), index, "create index"));
			log.Info("������ {0} ������ � ������� {1}", index.Name, index.PartitionName);
		}

		/// <summary>
		/// ������� ������
		/// </summary>
		/// <param name="index">������</param>
		public void DropIndex(Index index)
		{
			ExecuteNonQuery(razor.Parse(TemplateManager.GetDropIndexTemplate(), index, "drop index"));
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
				ExecuteNonQuery(razor.Parse(TemplateManager.GetAddColumnTemplate(), new object[] { table, column }, "add column"));
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
			ExecuteNonQuery(razor.Parse(TemplateManager.GetCreateForeignKeyTemplate(), foreignKey, "create foreignkey"));
			log.Info("������� ���� ������ {0}", foreignKey);
		}

		/// <summary>
		/// ������� ������� ����
		/// </summary>
		/// <param name="foreignKey">������� ����</param>
		public void DropForeignKey(ForeignKey foreignKey)
		{
			ExecuteNonQuery(razor.Parse(TemplateManager.GetDropForeignKeyTemplate(), foreignKey, "drop foreignkey"));
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

				ExecuteScalar(razor.Parse(TemplateManager.GetInsertRowsTemplate(), new object[] { table, props, values, identity }, "insert rows"));
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

			ExecuteNonQuery(razor.Parse(TemplateManager.GetDropColumnTemplate(), new object[] { table, column }, "drop column"));
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
		/// ��������� ������
		/// </summary>
		/// <param name="strategy">���������</param>
		/// <param name="query">������</param>
		public void ExecuteQuery(SupportedStrategy strategy, string query)
		{
			if (strategy == Strategy || strategy == SupportedStrategy.Any)
			{
				ExecuteNonQuery(query);
				log.Info("�������� ���������������� ������");
				log.Info(query);
			}
			else
			{
				log.Info("�������� ���������������� ������");
			}
		}

		/// <summary>
		/// identity_insert off
		/// </summary>
		/// <param name="tableName">��� �������</param>
		/// <param name="cmd">��������</param>
		protected abstract void DisableIdentityInsert(string tableName, IDbCommand cmd);

		/// <summary>
		/// ���������� ��� ���������
		/// </summary>
		/// <param name="headers">���������</param>
		/// <param name="i">�� ���������</param>
		/// <returns>��� ���������</returns>
		protected abstract string GetParameterName(string[] headers, int i);

		/// <summary>
		/// identity_insert on
		/// </summary>
		/// <param name="tableName">��� �������</param>
		/// <param name="cmd">��������</param>
		protected abstract void EnableIdentityInsert(string tableName, IDbCommand cmd);

		/// <summary>
		/// ��������� ������
		/// </summary>
		/// <param name="commandText">������</param>
		/// <returns>���������</returns>
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
		/// ��������� ������
		/// </summary>
		/// <param name="commandText">������</param>
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