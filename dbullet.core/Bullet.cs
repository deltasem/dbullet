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

		/// <summary>
		/// ������� ������� ����
		/// </summary>
		/// <param name="foreignKey">������� ����</param>
		public void CreateForeignKey(ForeignKey foreignKey)
		{
			Executor.DatabaseStrategy.CreateForeignKey(foreignKey);
		}

		/// <summary>
		/// ������� ������� ����
		/// </summary>
		/// <param name="foreignKey">������� ����</param>
		public void DropForeignKey(ForeignKey foreignKey)
		{
			Executor.DatabaseStrategy.DropForeignKey(foreignKey);
		}

		/// <summary>
		/// ������� ������� ����
		/// </summary>
		/// <param name="name">��� �����</param>
		/// <param name="tableName">������� � ������</param>
		public void DropForeignKey(string name, string tableName)
		{
			DropForeignKey(new ForeignKey(name, tableName, string.Empty, string.Empty, string.Empty));
		}

		/// <summary>
		/// ��������� ������ � �������
		/// </summary>
		/// <param name="table">�������</param>
		/// <param name="rows">������</param>
		public void InsertRows(string table, params object[] rows)
		{
			Executor.DatabaseStrategy.InsertRows(table, rows);
		}
	}
}
