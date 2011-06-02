//-----------------------------------------------------------------------
// <copyright file="Default.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// ������ ��� ����
	/// </summary>
	public class Default : Constraint
	{
		/// <summary>
		/// ��������
		/// </summary>
		private readonly string value;

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="value">��������</param>
		public Default(string name, string value) : base(name)
		{
			this.value = value;
		}

		/// <summary>
		/// ��������
		/// </summary>
		public string Value
		{
			get { return value; }
		}
	}
}