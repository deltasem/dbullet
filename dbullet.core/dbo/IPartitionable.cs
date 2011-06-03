//-----------------------------------------------------------------------
// <copyright file="IPartitionable.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Партиция
	/// </summary>
	public interface IPartitionable
	{
		/// <summary>
		/// Название партиции
		/// </summary>
		string PartitionName { get; }
	}
}