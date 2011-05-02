namespace dbullet.core.dbs
{
	using dbullet.core.dbo;

	/// <summary>
	/// ������������ �������� ��� ������ � �����
	/// </summary>
	public interface IDatabaseStrategy
	{
		/// <summary>
		/// ������ �������
		/// </summary>
		/// <param name="table">�������</param>
		void CreateTable(Table table);

		/// <summary>
		/// ���������� �� �������
		/// </summary>
		/// <param name="tableName">�������� �������</param>
		/// <returns>true - ���� ����������, ����� false</returns>
		bool IsTableExist(string tableName);
	}
}
