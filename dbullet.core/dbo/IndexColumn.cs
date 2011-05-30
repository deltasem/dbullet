//-----------------------------------------------------------------------
// <copyright file="IndexColumn.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// ������� ��� ���������� � ������
	/// </summary>
	public class IndexColumn : DatabaseObjectBase
	{
		/// <summary>
		/// ����������� ����������
		/// </summary>
		private readonly Direction direction;

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="direction">����������� ����������</param>
		public IndexColumn(string name, Direction direction = Direction.Ascending) : base(name)
		{
			this.direction = direction;
		}

		/// <summary>
		/// ����������� ����������
		/// </summary>
		public Direction Direction
		{
			get { return direction; }
		}
	}
}