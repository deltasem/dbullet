//-----------------------------------------------------------------------
// <copyright file="MsSql2008SysStrategy.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using dbullet.core.dbo;
using dbullet.core.dbs;

namespace dbullet.core.engine
{
	/// <summary>
	/// ���������� ��� MS SQL 2008 ���������
	/// </summary>
	public class MsSql2008SysStrategy : ISysDatabaseStrategy
	{
		/// <summary>
		/// ����������� � ����
		/// </summary>
		private readonly SqlConnection connection;

		/// <summary>
		/// ��������� ������ � �����
		/// </summary>
		private readonly MsSql2008Strategy sqlStrategy;

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="connection">���������� � ��</param>
		public MsSql2008SysStrategy(SqlConnection connection)
		{
			this.connection = connection;
			sqlStrategy = new MsSql2008Strategy(this.connection);
		}

		/// <summary>
		/// ������������� ���� ������
		/// ����������, ���� ��� ��������� �������
		/// </summary>
		public void InitDatabase()
		{
			if (!sqlStrategy.IsTableExist("dbullet"))
			{
				// todo: �������� ����� ������� �������� ������ � ������, ����� ����� ����� ������� "�����������" ��������
				// todo: �������� ����� �������� ���� ���������� ����������
				sqlStrategy.CreateTable(new Table("dbullet").AddColumn(new Column("Version", DbType.Int32)));
			}
		}

		/// <summary>
		/// ���������� ��������� ������ ����
		/// </summary>
		/// <returns>������ ����</returns>
		public int GetLastVersion()
		{
			try
			{
				connection.Open();
				using (var cmd = new SqlCommand("select max(Version) from dbullet", connection))
				{
					return int.Parse(cmd.ExecuteScalar().ToString());
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
		/// ��������� ������� ������
		/// </summary>
		/// <param name="version">������</param>
		public void SetCurrentVersion(int version)
		{
			try
			{
				connection.Open();
				using (var cmd = new SqlCommand(string.Empty, connection))
				{
					cmd.CommandText = string.Format("insert into dbullet({0})", version);
					cmd.ExecuteScalar();
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
