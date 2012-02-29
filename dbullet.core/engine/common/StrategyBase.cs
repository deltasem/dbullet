//-----------------------------------------------------------------------
// <copyright file="StrategyBase.cs" company="SofTrust" author="SKiryshin">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Data;

namespace dbullet.core.engine.common
{
	/// <summary>
	/// ������� ���������
	/// </summary>
	public class StrategyBase
	{
		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="connection">���������� � ��</param>
		public StrategyBase(IDbConnection connection)
		{
			this.connection = connection;
		}

		/// <summary>
		/// ����������� � ����
		/// </summary>
		protected IDbConnection connection { get; private set; }

		/// <summary>
		/// ��������� ������
		/// </summary>
		/// <param name="commandText">������</param>
		/// <returns>���������</returns>
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
		/// ��������� ������
		/// </summary>
		/// <param name="commandText">������</param>
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