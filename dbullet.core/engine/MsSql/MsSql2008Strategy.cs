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
	/// ��������� ������ � ����� MS SQL 2008
	/// </summary>
	public class MsSql2008Strategy : StrategyBase, IDatabaseStrategy
	{
		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="connection">���������� � ��</param>
		public MsSql2008Strategy(IDbConnection connection) : base(connection, new MsSqlTemplateManager())
		{
		}
	}
}
