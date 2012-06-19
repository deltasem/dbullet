//-----------------------------------------------------------------------
// <copyright file="OracleStrategy.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Data;
using dbullet.core.dbs;
using dbullet.core.engine.common;

namespace dbullet.core.engine.Oracle
{
	/// <summary>
	/// Оракловая стратегия
	/// </summary>
	public class OracleStrategy : StrategyBase, IDatabaseStrategy
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="connection">Соединение с БД</param>
		public OracleStrategy(IDbConnection connection) : base(connection, new OracleTemplateManager())
		{
		}

		/// <summary>
		/// Стратегия
		/// </summary>
		public override SupportedStrategy Strategy
		{
			get { return SupportedStrategy.Oracle; }
		}

		/// <summary>
		/// identity_insert off
		/// </summary>
		/// <param name="tableName">Имя таблицы</param>
		/// <param name="cmd">Комманда</param>
		protected override void DisableIdentityInsert(string tableName, IDbCommand cmd)
		{
		}

		/// <summary>
		/// identity_insert on
		/// </summary>
		/// <param name="tableName">Имя таблицы</param>
		/// <param name="cmd">Комманда</param>
		protected override void EnableIdentityInsert(string tableName, IDbCommand cmd)
		{
		}

		/// <summary>
		/// Возвращает имя параметра
		/// </summary>
		/// <param name="headers">Заголовки</param>
		/// <param name="i">ИД параметра</param>
		/// <returns>Имя параметра</returns>
		protected override string GetParameterName(string[] headers, int i)
		{
			return string.Format(":{0}", i + 1);
		}
	}
}