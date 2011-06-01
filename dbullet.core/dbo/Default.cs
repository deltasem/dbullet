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
		/// <param name="value">��������</param>
		public Default(string value) : base(string.Empty)
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