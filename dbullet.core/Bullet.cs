//-----------------------------------------------------------------------
// <copyright file="Bullet.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
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
			Executor.DatabaseStrategy.CreateTable(table);
		}

		/// <summary>
		/// ���������� �� �������
		/// </summary>
		/// <param name="tableName">�������� �������</param>
		/// <returns>true - ���� ����������, ����� false</returns>
		public bool IsTableExist(string tableName)
		{
			return Executor.DatabaseStrategy.IsTableExist(tableName);
		}

		/// <summary>
		/// ������ ������
		/// </summary>
		/// <param name="index">������</param>
		public void CreateIndex(Index index)
		{
			Executor.DatabaseStrategy.CreateIndex(index);
		}

		/// <summary>
		/// ������� �������
		/// </summary>
		/// <param name="tableName">�������� �������</param>
		public void DropTable(string tableName)
		{
			Executor.DatabaseStrategy.DropTable(tableName);
		}

		/// <summary>
		/// ������� ������
		/// </summary>
		/// <param name="index">������</param>
		public void DropIndex(Index index)
		{
			Executor.DatabaseStrategy.DropIndex(index);
		}
	}
}
