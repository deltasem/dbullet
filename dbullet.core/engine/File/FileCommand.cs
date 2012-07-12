//-----------------------------------------------------------------------
// <copyright file="FileCommand.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace dbullet.core.engine.File
{
	/// <summary>
	/// Комманда в файл
	/// </summary>
	public class FileCommand : IDbCommand
	{
		/// <summary>
		/// Соединение
		/// </summary>
		private FileConnection connection;

		/// <summary>
		/// Параметры
		/// </summary>
		private IDataParameterCollection parameters;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="connection">Соединение</param>
		public FileCommand(FileConnection connection)
		{
			Connection = connection;
			parameters = new FileDataParameterCollection(); 
		}

		/// <summary>
		/// Gets or sets the <see cref="T:System.Data.IDbConnection"/> used by this instance of the <see cref="T:System.Data.IDbCommand"/>.
		/// </summary>
		/// <returns>
		/// The connection to the data source.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public IDbConnection Connection
		{
			get
			{
				return connection;
			}

			set
			{
				connection = value as FileConnection;
			}
		}

		/// <summary>
		/// Gets or sets the transaction within which the Command object of a .NET Framework data provider executes.
		/// </summary>
		/// <returns>
		/// the Command object of a .NET Framework data provider executes. The default value is null.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public IDbTransaction Transaction { get; set; }

		/// <summary>
		/// Gets or sets the text command to run against the data source.
		/// </summary>
		/// <returns>
		/// The text command to execute. The default value is an empty string ("").
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public string CommandText { get; set; }

		/// <summary>
		/// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
		/// </summary>
		/// <returns>
		/// The time (in seconds) to wait for the command to execute. The default value is 30 seconds.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">The property value assigned is less than 0. </exception><filterpriority>2</filterpriority>
		public int CommandTimeout { get; set; }

		/// <summary>
		/// Indicates or specifies how the <see cref="P:System.Data.IDbCommand.CommandText"/> property is interpreted.
		/// </summary>
		/// <returns>
		/// One of the <see cref="T:System.Data.CommandType"/> values. The default is Text.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public CommandType CommandType { get; set; }

		/// <summary>
		/// Gets the <see cref="T:System.Data.IDataParameterCollection"/>.
		/// </summary>
		/// <returns>
		/// The parameters of the SQL statement or stored procedure.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public IDataParameterCollection Parameters
		{
			get
			{
				return parameters;
			}
		}

		/// <summary>
		/// Gets or sets how command results are applied to the <see cref="T:System.Data.DataRow"/> when used by the <see cref="M:System.Data.IDataAdapter.Update(System.Data.DataSet)"/> method of a <see cref="T:System.Data.Common.DbDataAdapter"/>.
		/// </summary>
		/// <returns>
		/// One of the <see cref="T:System.Data.UpdateRowSource"/> values. The default is Both unless the command is automatically generated. Then the default is None.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">The value entered was not one of the <see cref="T:System.Data.UpdateRowSource"/> values. </exception><filterpriority>2</filterpriority>
		public UpdateRowSource UpdatedRowSource { get; set; }

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
		}

		/// <summary>
		/// Creates a prepared (or compiled) version of the command on the data source.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Data.OleDb.OleDbCommand.Connection"/> is not set.-or- The <see cref="P:System.Data.OleDb.OleDbCommand.Connection"/> is not <see cref="M:System.Data.OleDb.OleDbConnection.Open"/>. </exception><filterpriority>2</filterpriority>
		public void Prepare()
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Attempts to cancels the execution of an <see cref="T:System.Data.IDbCommand"/>.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Cancel()
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Creates a new instance of an <see cref="T:System.Data.IDbDataParameter"/> object.
		/// </summary>
		/// <returns>
		/// An IDbDataParameter object.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public IDbDataParameter CreateParameter()
		{
			return new FileDataParameter();
		}

		/// <summary>
		/// Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
		/// </summary>
		/// <returns>
		/// The number of rows affected.
		/// </returns>
		/// <exception cref="T:System.InvalidOperationException">The connection does not exist.-or- The connection is not open. </exception><filterpriority>2</filterpriority>
		public int ExecuteNonQuery()
		{
			var p = (FileDataParameterCollection)parameters;
			string compiledCommandText = CommandText;
			foreach (var param in p.List)
			{
				compiledCommandText = compiledCommandText.Replace(param.ParameterName, string.Format("'{0}'", param.Value));
			}

			AppendScript(compiledCommandText);
			return 0;
		}

		/// <summary>
		/// Executes the <see cref="P:System.Data.IDbCommand.CommandText"/> against the <see cref="P:System.Data.IDbCommand.Connection"/> and builds an <see cref="T:System.Data.IDataReader"/>.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Data.IDataReader"/> object.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public IDataReader ExecuteReader()
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Executes the <see cref="P:System.Data.IDbCommand.CommandText"/> against the <see cref="P:System.Data.IDbCommand.Connection"/>, and builds an <see cref="T:System.Data.IDataReader"/> using one of the <see cref="T:System.Data.CommandBehavior"/> values.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Data.IDataReader"/> object.
		/// </returns>
		/// <param name="behavior">One of the <see cref="T:System.Data.CommandBehavior"/> values. </param><filterpriority>2</filterpriority>
		public IDataReader ExecuteReader(CommandBehavior behavior)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Executes the query, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
		/// </summary>
		/// <returns>
		/// The first column of the first row in the resultset.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public object ExecuteScalar()
		{
			AppendScript(CommandText);
			return 0;
		}

		/// <summary>
		/// Пишет скрипт в поток
		/// </summary>
		/// <param name="script">Скрипт</param>
		private void AppendScript(string script)
		{
			if (!script.EndsWith(";"))
			{
				script += ";";
			}

			connection.Output.AppendLine(script);
		}
	}
}