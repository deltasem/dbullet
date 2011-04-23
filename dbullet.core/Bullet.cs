namespace dbullet.core
{
	/// <summary>
	/// ��������, ������� ����� ���������� ��� ��
	/// </summary>
	public abstract class Bullet
	{
		/// <summary>
		/// ������
		/// </summary>
		public abstract int Version { get; }

		/// <summary>
		/// ����������
		/// </summary>
		public abstract void Update();

		/// <summary>
		/// ������ ����������
		/// </summary>
		public abstract void Downgrade();
	}
}