//-----------------------------------------------------------------------
// <copyright file="ITemplateManager.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.engine.common
{
	/// <summary>
	/// �������� ����������
	/// </summary>
	public interface ITemplateManager
	{
		/// <summary>
		/// ���������� ������ ��� �������� �������
		/// </summary>
		/// <returns>������</returns>
		string GetCreateTableTemplate();

		/// <summary>
		/// ���������� ������ ��� �������� ������������� �������
		/// </summary>
		/// <returns>������</returns>
		string GetIsTableExistTemplate();

		/// <summary>
		/// ���������� ������ ��� �������� ������������� �������
		/// </summary>
		/// <returns>������</returns>
		string GetIsColumnExistTemplate();

		/// <summary>
		/// ���������� ������ ��� �������� �������
		/// </summary>
		/// <returns>������</returns>
		string GetCreateIndexTemplate();

		/// <summary>
		/// ���������� ������ ��� �������� �������
		/// </summary>
		/// <returns>������</returns>
		string GetDropTableTemplate();

		/// <summary>
		/// ���������� ������ ��� �������� �������
		/// </summary>
		/// <returns>������</returns>
		string GetDropIndexTemplate();

		/// <summary>
		/// ���������� ������ ��� �������� �������� �����
		/// </summary>
		/// <returns>������</returns>
		string GetCreateForeignKeyTemplate();

		/// <summary>
		/// ���������� ������ ��� �������� �������� �����
		/// </summary>
		/// <returns>������</returns>
		string GetDropForeignKeyTemplate();

		/// <summary>
		/// ���������� ������ ��� ������� �������
		/// </summary>
		/// <returns>������</returns>
		string GetInsertRowsTemplate();

		/// <summary>
		/// ���������� ������ ��� ������� ������� �������
		/// </summary>
		/// <returns>������</returns>
		string GetInsertRowsStreamTemplate();

		/// <summary>
		/// ���������� ������ ��� ���������� �������
		/// </summary>
		/// <returns>������</returns>
		string GetAddColumnTemplate();

		/// <summary>
		/// ���������� ������ �������� �������
		/// </summary>
		/// <returns>������</returns>
		string GetDropColumnTemplate();

		/// <summary>
		/// Returns template for delete row script
		/// </summary>
		/// <returns>Delete row template</returns>
		string GetDeleteRowsTemplate();
	}
}