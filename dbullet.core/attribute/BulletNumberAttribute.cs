//-----------------------------------------------------------------------
// <copyright file="BulletNumberAttribute.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace dbullet.core.attribute
{
	/// <summary>
	/// Все булеты должны быть помечены этим аттрибутом
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class BulletNumberAttribute : Attribute
	{
		/// <summary>
		/// Ревизия булета
		/// </summary>
		private readonly int revision;

		/// <summary>
		/// Этим аттрибутом нужно помечать булеты
		/// </summary>
		/// <param name="revision">Ревизия булета</param>
		public BulletNumberAttribute(int revision)
		{
			this.revision = revision;
		}

		/// <summary>
		/// Ревизия булета
		/// </summary>
		public int Revision
		{
			get { return revision; }
		}
	}
}
