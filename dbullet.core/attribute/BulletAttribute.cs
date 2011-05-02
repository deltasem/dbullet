using System;

namespace dbullet.core.attribute
{
	/// <summary>
	/// ��� ������ ������ ���� �������� ���� ����������
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class BulletAttribute : Attribute
	{
		/// <summary>
		/// ������� ������
		/// </summary>
		private readonly int revision;

		/// <summary>
		/// ���� ���������� ����� �������� ������
		/// </summary>
		/// <param name="revision">������� ������</param>
		public BulletAttribute(int revision)
		{
			this.revision = revision;
		}

		/// <summary>
		/// ������� ������
		/// </summary>
		public int Revision
		{
			get { return revision; }
		}
	}
}
