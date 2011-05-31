//-----------------------------------------------------------------------
// <copyright file="ForeignAction.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// �������� ��� ��������
	/// </summary>
	public enum ForeignAction
	{
		/// <summary>
		/// ��� ��������
		/// </summary>
		NoAction,
		
		/// <summary>
		/// ��������� ��������
		/// </summary>
		Cascade,

		/// <summary>
		/// ��������� null
		/// </summary>
		SetNull,
	}
}