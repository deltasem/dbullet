using System;
using System.Data.SqlClient;

namespace dbullet.core.dbs
{
	/// <summary>
	/// ���������� ��� MS SQL 2008 ���������
	/// </summary>
	internal class MsSql2008SysStrategy : ISysDatabaseStrategy
	{
		/// <summary>
		/// ����������� � ����
		/// </summary>
		private readonly SqlConnection connection;

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="connection">���������� � ��</param>
		public MsSql2008SysStrategy(SqlConnection connection)
		{
			this.connection = connection;
		}

		/// <summary>
		/// ������������� ���� ������
		/// ����������, ���� ��� ��������� �������
		/// </summary>
		public void InitDatabase()
		{
		}

		/// <summary>
		/// ���������� ��������� ������ ����
		/// </summary>
		/// <returns>������ ����</returns>
		public int GetLastVersion()
		{
			return 0;
		}

		/// <summary>
		/// ��������� ������� ������
		/// </summary>
		public void SetCurrentVersion()
		{
		}
	}
}
