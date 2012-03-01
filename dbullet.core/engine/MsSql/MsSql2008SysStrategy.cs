//-----------------------------------------------------------------------
// <copyright file="MsSql2008SysStrategy.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Data;
using dbullet.core.dbs;
using dbullet.core.engine.common;

namespace dbullet.core.engine.MsSql
{
	/// <summary>
	/// Реализация длы MS SQL 2008 стратегии
	/// </summary>
	public class MsSql2008SysStrategy : SysStrategyBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="connection">Соединение с БД</param>
		/// <param name="strategy">Стратегия работы с БД</param>
		public MsSql2008SysStrategy(IDbConnection connection, IDatabaseStrategy strategy) : base(connection, strategy)
		{
		}
	}
}
