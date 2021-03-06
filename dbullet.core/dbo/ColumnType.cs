//-----------------------------------------------------------------------
// <copyright file="ColumnType.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	using System.Data;

	/// <summary>
	/// Тип колонки
	/// </summary>
	public class ColumnType
	{
		/// <summary>
		/// Тип базы данных
		/// </summary>
		private readonly DbType? dbType;

		/// <summary>
		/// Тип поля
		/// </summary>
		private readonly SqlDbType? sqlDbType;

		/// <summary>
		/// Размерность
		/// </summary>
		private readonly int length;

		/// <summary>
		/// Хреновина после запятой
		/// </summary>
		private readonly int scale;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbType">Тип БД</param>
		public ColumnType(DbType dbType)
			: this(dbType, 0, 0)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbType">Тип БД</param>
		/// <param name="length">Размерность</param>
		public ColumnType(DbType dbType, int length)
			: this(dbType, length, 0)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbType">Тип БД</param>
		/// <param name="length">Размерность</param>
		/// <param name="scale">Разрядность</param>
		public ColumnType(DbType dbType, int length, int scale)
		{
			this.dbType = dbType;
			this.length = length;
			this.scale = scale;
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbType">Тип БД</param>
		public ColumnType(SqlDbType dbType)
			: this(dbType, 0, 0)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbType">Тип БД</param>
		/// <param name="length">Размерность</param>
		public ColumnType(SqlDbType dbType, int length)
			: this(dbType, length, 0)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbType">Тип БД</param>
		/// <param name="length">Размерность</param>
		/// <param name="scale">Разрядность</param>
		public ColumnType(SqlDbType dbType, int length, int scale)
		{
			this.sqlDbType = dbType;
			this.length = length;
			this.scale = scale;
		}

		/// <summary>
		/// Тип БД
		/// </summary>
		public DbType? DbType
		{
			get
			{
				return this.dbType;
			}
		}

		/// <summary>
		/// Тип БД
		/// </summary>
		public SqlDbType? SqlDbType
		{
			get
			{
				return this.sqlDbType;
			}
		}

		/// <summary>
		/// Размерность
		/// </summary>
		public int Length
		{
			get
			{
				return this.length;
			}
		}

		/// <summary>
		/// Разрядность
		/// </summary>
		public int Scale
		{
			get
			{
				return this.scale;
			}
		}
	}
}
