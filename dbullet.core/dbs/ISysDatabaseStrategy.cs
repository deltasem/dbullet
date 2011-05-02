namespace dbullet.core.dbs
{
	/// <summary>
	/// ������ ��� ��������� �����������
	/// ���������� / ���������� ������, ������������� ��������� �������
	/// </summary>
	internal interface ISysDatabaseStrategy
	{
		/// <summary>
		/// ������������� ���� ������
		/// ����������, ���� ��� ��������� �������
		/// </summary>
		void InitDatabase();

		/// <summary>
		/// ���������� ��������� ������ ����
		/// </summary>
		/// <returns>������ ����</returns>
		int GetLastVersion();

		/// <summary>
		/// ��������� ������� ������
		/// </summary>
		void SetCurrentVersion();
	}
}
