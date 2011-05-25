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
		/// Значение по умолчанию
		/// </summary>
		private readonly string defaultValue;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название столбца</param>
		/// <param name="columnType">Тип столбца</param>
		public Column(string name, ColumnType columnType)
			: this(name, columnType, true, string.Empty)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название столбца</param>
		/// <param name="dbType">Тип столбца</param>
		public Column(string name, DbType dbType)
			: this(name, new ColumnType(dbType), true, string.Empty)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Имя столбца</param>
		/// <param name="columnType">Тип столбца</param>
		/// <param name="defaultValue">Значение по умолчанию</param>
		public Column(string name, ColumnType columnType, string defaultValue)
			: this(name, columnType, true, defaultValue)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Имя столбца</param>
		/// <param name="columnType">Тип столбца</param>
		/// <param name="nullable">Может содержать null</param>
		public Column(string name, ColumnType columnType, bool nullable)
			: this(name, columnType, nullable, string.Empty)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Имя столбца</param>
		/// <param name="columnType">Тип столбца</param>
		/// <param name="nullable">Может содержать null</param>
		/// <param name="defaultValue">Значение по умолчанию</param>
		public Column(string name, ColumnType columnType, bool nullable, string defaultValue)
			: base(name)
		{
			this.columnType = columnType;
			this.nullable = nullable;
			this.defaultValue = defaultValue;
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

		/// <summary>
		/// Значение по умолчанию
		/// </summary>
		public string DefaultValue
		{
			get
			{
				return defaultValue;
			}
		}
	}
}
