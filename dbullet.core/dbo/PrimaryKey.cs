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
	public class PrimaryKey : Constraint, IPartitionable
	{
		/// <summary>
		/// Название партиции
		/// </summary>
		private readonly string partitionName;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название ключа</param>
		public PrimaryKey(string name) : base(name)
		{
			partitionName = "PRIMARY";
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название ключа</param>
		/// <param name="partitionName">Партиция</param>
		public PrimaryKey(string name, string partitionName) : base(name)
		{
			this.partitionName = partitionName;
		}

		#region Implementation of IPartitionable
		/// <summary>
		/// Название партиции
		/// </summary>
		public string PartitionName
		{
			get { return partitionName; }
		}
		#endregion
	}
}
