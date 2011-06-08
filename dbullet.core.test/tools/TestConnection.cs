//-----------------------------------------------------------------------
// <copyright file="TestConnection.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;

namespace dbullet.core.test.tools
{
	/// <summary>
	/// Загулшка для подключения
	/// </summary>
	public class TestConnection : IDbConnection
	{
		/// <summary>
		/// Список комманд
		/// </summary>
		private readonly List<string> allCommands = new List<string>();

		/// <summary>
		/// Параметры SQL
		/// </summary>
		private readonly TestDataParametrCollection dbDataParametrs = new TestDataParametrCollection();

		/// <summary>
		/// Последний запрос
		/// </summary>
		private string lastCommandText;

		/// <summary>
		/// Состояние
		/// </summary>
		private ConnectionState state;

		/// <summary>
		/// Значение для запроса без результата
		/// </summary>
		public int ExecuteNonQueryValue { get; set; }

		/// <summary>
		/// Последний запрос
		/// </summary>
		public string LastCommandText
		{
			get
			{
				return lastCommandText;
			}

			set
			{
				lastCommandText = value.Replace("\r", string.Empty).Replace("\n", string.Empty);
				allCommands.Add(lastCommandText);
			}
		}

		/// <summary>
		/// Список комманд
		/// </summary>
		public List<string> AllCommands
		{
			get { return allCommands; }
		}

		/// <summary>
		/// Значение для ExecuteScalar
		/// </summary>
		public object ExecuteScalarValue { get; set; }

		#region Implementation of IDbConnection

		/// <summary>
		/// Gets or sets the string used to open a database.
		/// </summary>
		/// <returns>
		/// A string containing connection settings.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public string ConnectionString { get; set; }

		/// <summary>
		/// Gets the time to wait while trying to establish a connection before terminating the attempt and generating an error.
		/// </summary>
		/// <returns>
		/// The time (in seconds) to wait for a connection to open. The default value is 15 seconds.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public int ConnectionTimeout
		{
			get { return 0; }
		}

		/// <summary>
		/// Gets the name of the current database or the database to be used after a connection is opened.
		/// </summary>
		/// <returns>
		/// The name of the current database or the name of the database to be used once a connection is open. The default value is an empty string.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public string Database
		{
			get { return String.Empty; }
		}

		/// <summary>
		/// Gets the current state of the connection.
		/// </summary>
		/// <returns>
		/// One of the <see cref="T:System.Data.ConnectionState"/> values.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public ConnectionState State
		{
			get { return state; }
		}

		/// <summary>
		/// SQL параметры
		/// </summary>
		public TestDataParametrCollection DbDataParametrs
		{
			get { return dbDataParametrs; }
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
		}

		/// <summary>
		/// Begins a database transaction.
		/// </summary>
		/// <returns>
		/// An object representing the new transaction.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public IDbTransaction BeginTransaction()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Begins a database transaction with the specified <see cref="T:System.Data.IsolationLevel"/> value.
		/// </summary>
		/// <returns>
		/// An object representing the new transaction.
		/// </returns>
		/// <param name="il">One of the <see cref="T:System.Data.IsolationLevel"/> values. </param><filterpriority>2</filterpriority>
		public IDbTransaction BeginTransaction(IsolationLevel il)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Closes the connection to the database.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Close()
		{
			state = ConnectionState.Closed;
		}

		/// <summary>
		/// Changes the current database for an open Connection object.
		/// </summary>
		/// <param name="databaseName">The name of the database to use in place of the current database. </param><filterpriority>2</filterpriority>
		public void ChangeDatabase(string databaseName)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates and returns a Command object associated with the connection.
		/// </summary>
		/// <returns>
		/// A Command object associated with the connection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public IDbCommand CreateCommand()
		{
			return new TestCommand(this);
		}

		/// <summary>
		/// Opens a database connection with the settings specified by the ConnectionString property of the provider-specific Connection object.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Open()
		{
			state = ConnectionState.Open;
		}

		#endregion
	}
}