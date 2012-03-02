//-----------------------------------------------------------------------
// <copyright file="MsSql2008Strategy.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Data;
using dbullet.core.dbs;
using dbullet.core.engine.common;

namespace dbullet.core.engine.MsSql
{
	/// <summary>
	/// Стратегия работы с базой MS SQL 2008
	/// </summary>
	public class MsSql2008Strategy : StrategyBase, IDatabaseStrategy
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="connection">Соединение с БД</param>
		public MsSql2008Strategy(IDbConnection connection) : base(connection, new MsSqlTemplateManager())
		{
		}

		/// <summary>
		/// identity_insert off
		/// </summary>
		/// <param name="tableName">Имя таблицы</param>
		/// <param name="cmd">Комманда</param>
		protected override void DisableIdentityInsert(string tableName, IDbCommand cmd)
		{
			cmd.CommandText = string.Format("set identity_insert [{0}] off;", tableName);
			cmd.ExecuteNonQuery();
		}

		/// <summary>
		/// identity_insert on
		/// </summary>
		/// <param name="tableName">Имя таблицы</param>
		/// <param name="cmd">Комманда</param>
		protected override void EnableIdentityInsert(string tableName, IDbCommand cmd)
		{
			cmd.CommandText = string.Format("set identity_insert [{0}] on;", tableName);
			cmd.ExecuteNonQuery();
		}

		/// <summary>
		/// Возвращает имя параметра
		/// </summary>
		/// <param name="headers">Заголовки</param>
		/// <param name="i">ИД параметра</param>
		/// <returns>Имя параметра</returns>
		protected override string GetParameterName(string[] headers, int i)
		{
			return string.Format("@{0}", headers[i]);
		}
	}
}
