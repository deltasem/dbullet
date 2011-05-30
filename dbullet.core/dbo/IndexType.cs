//-----------------------------------------------------------------------
// <copyright file="IndexType.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Типы индексов
	/// </summary>
	public enum IndexType
	{
		/// <summary>
		/// Некластеризованый индекс
		/// </summary>
		Nonclustered,

		/// <summary>
		/// Кластеризованый индекс
		/// </summary>
		Clustered
	}
}