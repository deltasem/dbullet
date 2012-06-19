//-----------------------------------------------------------------------
// <copyright file="FileConnection.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Data;
using System.Text;

namespace dbullet.core.engine.File
{
	/// <summary>
	/// Скрипты в файл
	/// </summary>
	public class FileConnection : IDbConnection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		/// <param name="connectionString"> Папка со скирптами </param>
		public FileConnection(string connectionString)
		{
			Output = new StringBuilder();
			ConnectionString = connectionString;
		}

		/// <summary>
		/// Поток
		/// </summary>
		public StringBuilder Output { get; set; }

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
		public int ConnectionTimeout { get; private set; }

		/// <summary>
		/// Gets the name of the current database or the database to be used after a connection is opened.
		/// </summary>
		/// <returns>
		/// The name of the current database or the name of the database to be used once a connection is open. The default value is an empty string.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public string Database { get; private set; }

		/// <summary>
		/// Gets the current state of the connection.
		/// </summary>
		/// <returns>
		/// One of the <see cref="T:System.Data.ConnectionState"/> values.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public ConnectionState State { get; private set; }

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
			throw new System.NotImplementedException();
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
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Closes the connection to the database.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Close()
		{
		}

		/// <summary>
		/// Changes the current database for an open Connection object.
		/// </summary>
		/// <param name="databaseName">The name of the database to use in place of the current database. </param><filterpriority>2</filterpriority>
		public void ChangeDatabase(string databaseName)
		{
			throw new System.NotImplementedException();
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
			return new FileCommand(this);
		}

		/// <summary>
		/// Opens a database connection with the settings specified by the ConnectionString property of the provider-specific Connection object.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Open()
		{
		}
	}
}