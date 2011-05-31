//-----------------------------------------------------------------------
// <copyright file="ITemplateManager.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.engine
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
		/// ���������� ������ ��� �������� �������
		/// </summary>
		/// <returns>������</returns>
		string GetCreateIndexTemplate();

		/// <summary>
		/// ���������� ������ ��� �������� �������
		/// </summary>
		/// <returns>������</returns>
		string GetDropTableTemplate();
	}
}