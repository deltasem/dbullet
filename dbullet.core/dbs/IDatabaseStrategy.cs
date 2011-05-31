//-----------------------------------------------------------------------
// <copyright file="IDatabaseStrategy.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbs
{
	using dbo;

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
		/// ������� �������
		/// </summary>
		/// <param name="tableName">�������� �������</param>
		void DropTable(string tableName);

		/// <summary>
		/// ���������� �� �������
		/// </summary>
		/// <param name="tableName">�������� �������</param>
		/// <returns>true - ���� ����������, ����� false</returns>
		bool IsTableExist(string tableName);

		/// <summary>
		/// ������ ������
		/// </summary>
		/// <param name="index">������</param>
		void CreateIndex(Index index);

		/// <summary>
		/// ������� ������
		/// </summary>
		/// <param name="index">������</param>
		void DropIndex(Index index);
	}
}
