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
	}
}