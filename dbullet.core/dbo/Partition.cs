﻿//-----------------------------------------------------------------------
// <copyright file="Partition.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Партишин
	/// </summary>
	public class Partition : DatabaseObjectBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название партишин</param>
		public Partition(string name) : base(name)
		{
		}
	}
}
