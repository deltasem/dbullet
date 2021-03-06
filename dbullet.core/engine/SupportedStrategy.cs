//-----------------------------------------------------------------------
// <copyright file="SupportedStrategy.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.engine
{
	/// <summary>
	/// Поддерживаемые стратегии работы с БД
	/// </summary>
	public enum SupportedStrategy
	{
		/// <summary>
		/// Microsoft SQL Server 2008
		/// </summary>
		Mssql2008, 

		/// <summary>
		/// Oracle
		/// </summary>
		Oracle,

		/// <summary>
		/// Microsoft SQL Server 2008 скрипты в файл
		/// </summary>
		Mssql2008File,

		/// <summary>
		/// Oracle скрипты в файл
		/// </summary>
		OracleFile,
		
		/// <summary>
		/// Любая стратегия
		/// </summary>
		Any
	}
}
