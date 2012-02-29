//-----------------------------------------------------------------------
// <copyright file="MsSql2008SysStrategy.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Data;
using dbullet.core.dbo;
using dbullet.core.dbs;

namespace dbullet.core.engine.MsSql
{
	/// <summary>
	/// ���������� ��� MS SQL 2008 ���������
	/// </summary>
	public class MsSql2008SysStrategy : ISysDatabaseStrategy
	{
		/// <summary>
		/// ����������� � ����
		/// </summary>
		private readonly IDbConnection connection;

		/// <summary>
		/// ��������� ������ � ��
		/// </summary>
		private readonly IDatabaseStrategy strategy;

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="connection">���������� � ��</param>
		/// <param name="strategy">��������� ������ � ��</param>
		public MsSql2008SysStrategy(IDbConnection connection, IDatabaseStrategy strategy)
		{
			this.connection = connection;
			this.strategy = strategy;
		}

		/// <summary>
		/// ������������� ���� ������
		/// ����������, ���� ��� ��������� �������
		/// </summary>
		/// <param name="name">���</param>
		public void InitDatabase(string name)
		{
			if (!this.strategy.IsTableExist("dbullet"))
			{
				this.strategy.CreateTable(new Table("dbullet").AddColumn(new Column("Version", DbType.Int32)));
			}

			if (!this.strategy.IsColumnExists("dbullet", "Assembly"))
			{
				var column = new Column("Assembly", DbType.String.Size(1024), false)
				             	{
				             		Constraint = new ValueDefault("dbullet_assembly_default", name)
				             	};
				this.strategy.AddColumn(new Table("dbullet"), column);
			}
		}

		/// <summary>
		/// ���������� ��������� ������ ����
		/// </summary>
		/// <param name="name">���</param>
		/// <returns>������ ����</returns>
		public int GetLastVersion(string name)
		{
			try
			{
				this.connection.Open();
				using (var cmd = this.connection.CreateCommand())
				{
					cmd.CommandText = string.Format("select max(Version) from dbullet where assembly = '{0}'", name);
					var res = cmd.ExecuteScalar();
					if (res == System.DBNull.Value)
					{
						return 0;
					}

					return int.Parse(cmd.ExecuteScalar().ToString());
				}
			}
			finally
			{
				if (this.connection != null)
				{
					this.connection.Close();
				}
			}
		}

		/// <summary>
		/// ��������� ������� ������
		/// </summary>
		/// <param name="version">������</param>
		/// <param name="name">���</param>
		public void SetCurrentVersion(int version, string name)
		{
			try
			{
				this.connection.Open();
				using (var cmd = this.connection.CreateCommand())
				{
					cmd.CommandText = string.Format("insert into dbullet(Version, Assembly) values({0}, '{1}')", version, name);
					cmd.ExecuteScalar();
				}
			}
			finally
			{
				if (this.connection != null)
				{
					this.connection.Close();
				}
			}
		}

		/// <summary>
		/// �������� ���������� �� ��������� ������
		/// </summary>
		/// <param name="version">������</param>
		/// <param name="name">���</param>
		public void RemoveVersionInfo(int version, string name)
		{
			try
			{
				this.connection.Open();
				using (var cmd = this.connection.CreateCommand())
				{
					cmd.CommandText = string.Format("delete from dbullet where version = {0} and Assembly = '{1}'", version, name);
					cmd.ExecuteScalar();
				}
			}
			finally
			{
				if (this.connection != null)
				{
					this.connection.Close();
				}
			}
		}
	}
}
