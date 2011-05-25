//-----------------------------------------------------------------------
// <copyright file="ISysDatabaseStrategy.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
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
		/// <param name="version">������</param>
		void SetCurrentVersion(int version);
	}
}
