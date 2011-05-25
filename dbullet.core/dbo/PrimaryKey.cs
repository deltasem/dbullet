//-----------------------------------------------------------------------
// <copyright file="PrimaryKey.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Первичный ключ
	/// </summary>
	public class PrimaryKey : PartitionableObject
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название ключа</param>
		public PrimaryKey(string name) : base(name, "PRIMARY")
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название ключа</param>
		/// <param name="partitionName">Партиция</param>
		public PrimaryKey(string name, string partitionName) : base(name, partitionName)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название ключа</param>
		/// <param name="partition">Партиция</param>
		public PrimaryKey(string name, Partition partition) : base(name, partition)
		{
		}
	}
}
