//-----------------------------------------------------------------------
// <copyright file="StrategyBase.cs" company="SofTrust" author="SKiryshin">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Data;

namespace dbullet.core.engine.common
{
	/// <summary>
	/// Базовая стратегия
	/// </summary>
	public class StrategyBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="connection">Соединение с БД</param>
		public StrategyBase(IDbConnection connection)
		{
			this.connection = connection;
		}

		/// <summary>
		/// Подключение к базе
		/// </summary>
		protected IDbConnection connection { get; private set; }

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