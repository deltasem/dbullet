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
	}
}