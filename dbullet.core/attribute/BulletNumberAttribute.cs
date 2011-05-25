//-----------------------------------------------------------------------
// <copyright file="BulletNumberAttribute.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace dbullet.core.attribute
{
	/// <summary>
	/// ��� ������ ������ ���� �������� ���� ����������
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class BulletNumberAttribute : Attribute
	{
		/// <summary>
		/// ������� ������
		/// </summary>
		private readonly int revision;

		/// <summary>
		/// ���� ���������� ����� �������� ������
		/// </summary>
		/// <param name="revision">������� ������</param>
		public BulletNumberAttribute(int revision)
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
