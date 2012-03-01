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
	/// ���������� ��� MS SQL 2008 ���������
	/// </summary>
	public class MsSql2008SysStrategy : SysStrategyBase
	{
		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="connection">���������� � ��</param>
		/// <param name="strategy">��������� ������ � ��</param>
		public MsSql2008SysStrategy(IDbConnection connection, IDatabaseStrategy strategy) : base(connection, strategy)
		{
		}
	}
}
