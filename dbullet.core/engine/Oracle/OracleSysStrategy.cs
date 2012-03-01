//-----------------------------------------------------------------------
// <copyright file="OracleSysStrategy.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Data;
using dbullet.core.dbs;
using dbullet.core.engine.common;

namespace dbullet.core.engine.Oracle
{
	/// <summary>
	/// Оракловая системная стратегия
	/// </summary>
	public class OracleSysStrategy : SysStrategyBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="connection">Соединение с БД</param>
		/// <param name="strategy">Стратегия работы с БД</param>
		public OracleSysStrategy(IDbConnection connection, IDatabaseStrategy strategy) : base(connection, strategy)
		{
		}
	}
}