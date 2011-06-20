//-----------------------------------------------------------------------
// <copyright file="StandartDefault.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// ��������� �������
	/// </summary>
	public class StandartDefault : Default
	{
		/// <summary>
		/// ��� �������
		/// </summary>
		private readonly StandartDefaultType defaultType;

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="defaultType">��� �������</param>
		public StandartDefault(string name, StandartDefaultType defaultType) : base(name)
		{
			this.defaultType = defaultType;
		}

		/// <summary>
		/// ��� �������
		/// </summary>
		public StandartDefaultType DefaultType
		{
			get { return defaultType; }
		}
	}
}