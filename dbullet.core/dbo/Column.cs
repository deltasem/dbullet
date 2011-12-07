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
		/// Идентити
		/// </summary>
		private readonly bool identity;
		
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Имя столбца</param>
		/// <param name="dbType">Тип столбца</param>
		/// <param name="nullable">Может содержать null</param>
		/// <param name="identity">Идентити</param>
		public Column(string name, DbType dbType, bool nullable = true, bool identity = false)
			: this(name, new ColumnType(dbType), nullable, identity)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Имя столбца</param>
		/// <param name="dbType">Тип столбца</param>
		/// <param name="nullable">Может содержать null</param>
		/// <param name="identity">Идентити</param>
		public Column(string name, SqlDbType dbType, bool nullable = true, bool identity = false)
			: this(name, new ColumnType(dbType), nullable, identity)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Имя столбца</param>
		/// <param name="columnType">Тип столбца</param>
		/// <param name="nullable">Может содержать null</param>
		/// <param name="identity">Идентити</param>
		public Column(string name, ColumnType columnType, bool nullable = true, bool identity = false)
			: base(name)
		{
			this.columnType = columnType;
			this.nullable = nullable;
			this.identity = identity;
		}

		/// <summary>
		/// Идентити
		/// </summary>
		public bool Identity
		{
			get
			{
				return identity;
			}
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
		/// Первичный ключ
		/// </summary>
		public Constraint Constraint { get; set; }
	}
}
