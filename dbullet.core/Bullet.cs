//-----------------------------------------------------------------------
// <copyright file="Bullet.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using dbullet.core.dbo;
using dbullet.core.dbs;
using dbullet.core.engine;

namespace dbullet.core
{
	/// <summary>
	/// ��������, ������� ����� ���������� ��� ��
	/// </summary>
	public abstract class Bullet : IDatabaseStrategy
	{
		/// <summary>
		/// ���������
		/// </summary>
		private readonly IDatabaseStrategy strategy = new MsSql2008Strategy(null);

		/// <summary>
		/// ����������
		/// </summary>
		public abstract void Update();

		/// <summary>
		/// ������ ����������
		/// </summary>
		public abstract void Downgrade();

		/// <summary>
		/// ������ �������
		/// </summary>
		/// <param name="table">�������</param>
		public void CreateTable(Table table)
		{
			strategy.CreateTable(table);
		}

		/// <summary>
		/// ���������� �� �������
		/// </summary>
		/// <param name="tableName">�������� �������</param>
		/// <returns>true - ���� ����������, ����� false</returns>
		public bool IsTableExist(string tableName)
		{
			return strategy.IsTableExist(tableName);
		}

		/// <summary>
		/// ������� �������
		/// </summary>
		/// <param name="tableName">�������� �������</param>
		public void DropTable(string tableName)
		{
			strategy.DropTable(tableName);
		}
	}
}
