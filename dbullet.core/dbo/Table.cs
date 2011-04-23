//-----------------------------------------------------------------------
// <copyright file="Table.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// �������
	/// </summary>
	public class Table : DatabaseObjectBase
	{
		/// <summary>
		/// �������
		/// </summary>
		private readonly Column[] columns;

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="columns">�������</param>
		public Table(string name, params Column[] columns) : base(name)
		{
			this.columns = columns;
		}

		/// <summary>
		/// �������
		/// </summary>
		public Column[] Columns
		{
			get
			{
				return this.columns;
			}
		}
	}
}