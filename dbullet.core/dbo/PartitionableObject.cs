//-----------------------------------------------------------------------
// <copyright file="PartitionableObject.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// ������, ������� ����� ���� ������� � �������� (�������, ������...)
	/// </summary>
	public abstract class PartitionableObject : DatabaseObjectBase
	{
		/// <summary>
		/// ��������
		/// </summary>
		private readonly Partition partition;

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="partitionName">��������</param>
		public PartitionableObject(string name, string partitionName) : base(name)
		{
			partition = new Partition(partitionName);
		}

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �������</param>
		/// <param name="partition">��������</param>
		public PartitionableObject(string name, Partition partition) : base(name)
		{
			this.partition = partition;
		}

		/// <summary>
		/// ��������
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