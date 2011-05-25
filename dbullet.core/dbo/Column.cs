//-----------------------------------------------------------------------
// <copyright file="Column.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Data;

namespace dbullet.core.dbo
{
	/// <summary>
	/// Столбец таблицы
	/// </summary>
	public class Column : DatabaseObjectBase
	{
		/// <summary>
		/// Тип столбца
		/// </summary>
		private readonly ColumnType columnType;

		/// <summary>
		/// Возможность вставлять null
		/// </summary>
		private readonly bool nullable;

		/// <summary>
		/// Первичный ключ
		/// </summary>
		private PrimaryKey primaryKey;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название столбца</param>
		/// <param name="columnType">Тип столбца</param>
		public Column(string name, ColumnType columnType)
			: this(name, columnType, true)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название столбца</param>
		/// <param name="dbType">Тип столбца</param>
		public Column(string name, DbType dbType)
			: this(name, new ColumnType(dbType), true)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Имя столбца</param>
		/// <param name="columnType">Тип столбца</param>
		/// <param name="nullable">Может содержать null</param>
		public Column(string name, ColumnType columnType, bool nullable)
			: base(name)
		{
			this.columnType = columnType;
			this.nullable = nullable;
		}

		/// <summary>
		/// Тип колонки
		/// </summary>
		public ColumnType ColumnType
		{
			get
			{
				return columnType;
			}
		}

		/// <summary>
		/// Можно ли вставлять null
		/// </summary>
		public bool Nullable
		{
			get
			{
				return nullable;
			}
		}
	}
}
