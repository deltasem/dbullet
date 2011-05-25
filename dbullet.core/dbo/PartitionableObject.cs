//-----------------------------------------------------------------------
// <copyright file="PartitionableObject.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Объект, который может быть вынесен в партицию (таблица, индекс...)
	/// </summary>
	public abstract class PartitionableObject : DatabaseObjectBase
	{
		/// <summary>
		/// Партишин
		/// </summary>
		private readonly Partition partition;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название таблицы</param>
		/// <param name="partitionName">Партиция</param>
		public PartitionableObject(string name, string partitionName) : base(name)
		{
			partition = new Partition(partitionName);
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название объекта</param>
		/// <param name="partition">Партиция</param>
		public PartitionableObject(string name, Partition partition) : base(name)
		{
			this.partition = partition;
		}

		/// <summary>
		/// Партишин
		/// </summary>
		public Partition Partition
		{
			get
			{
				return partition;
			}
		}
	}
}