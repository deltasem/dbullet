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

		/// <summary>
		/// identity_insert off
		/// </summary>
		/// <param name="tableName">��� �������</param>
		/// <param name="cmd">��������</param>
		protected override void DisableIdentityInsert(string tableName, IDbCommand cmd)
		{
			cmd.CommandText = string.Format("set identity_insert [{0}] off;", tableName);
			cmd.ExecuteNonQuery();
		}

		/// <summary>
		/// identity_insert on
		/// </summary>
		/// <param name="tableName">��� �������</param>
		/// <param name="cmd">��������</param>
		protected override void EnableIdentityInsert(string tableName, IDbCommand cmd)
		{
			cmd.CommandText = string.Format("set identity_insert [{0}] on;", tableName);
			cmd.ExecuteNonQuery();
		}

		/// <summary>
		/// ���������� ��� ���������
		/// </summary>
		/// <param name="headers">���������</param>
		/// <param name="i">�� ���������</param>
		/// <returns>��� ���������</returns>
		protected override string GetParameterName(string[] headers, int i)
		{
			return string.Format("@{0}", headers[i]);
		}
	}
}
