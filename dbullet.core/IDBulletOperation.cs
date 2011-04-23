//-----------------------------------------------------------------------
// <copyright file="IDBulletOperation.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core
{
	/// <summary>
	/// ��������, ������� ����� ���������� ��� ��
	/// </summary>
	public interface IDBulletOperation
	{
		/// <summary>
		/// ���������� �������
		/// </summary>
		/// <param name="tableName">�������� �������</param>
		void CreateTable(string tableName);
	}
}