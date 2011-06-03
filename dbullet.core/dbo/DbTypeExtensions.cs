//-----------------------------------------------------------------------
// <copyright file="DbTypeExtensions.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	using System.Data;

	/// <summary>
	/// Расширения DbType
	/// </summary>
	public static class DbTypeExtensions
	{
		/// <summary>
		/// Размерность столбца
		/// </summary>
		/// <param name="dbType">Тип столбца ДБ</param>
		/// <param name="size">Размерность</param>
		/// <returns>Тип столбца</returns>
		public static ColumnType Size(this DbType dbType, int size)
		{
			return new ColumnType(dbType, size);
		}

		/// <summary>
		/// Размерность столбца
		/// </summary>
		/// <param name="dbType">Тип столбца ДБ</param>
		/// <param name="size">Размерность</param>
		/// <param name="scale">Разрядность</param>
		/// <returns>Тип столбца</returns>
		public static ColumnType Size(this DbType dbType, int size, int scale)
		{
			return new ColumnType(dbType, size, scale);
		}
	}
}
