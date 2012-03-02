//-----------------------------------------------------------------------
// <copyright file="Table.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Data;
using System.Linq;
using dbullet.core.exception;

namespace dbullet.core.dbo
{
	using System.Collections.Generic;

	/// <summary>
	/// Таблица
	/// </summary>
	public class Table : DatabaseObjectBase, IPartitionable
	{
		/// <summary>
		/// Колонки
		/// </summary>
		private readonly List<Column> columns;

		/// <summary>
		/// Название партиции
		/// </summary>
		private string partitionName;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название таблицы</param>
		public Table(string name) : this(name, string.Empty)
		{
		}
		
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название таблицы</param>
		/// <param name="partitionName">Партиция</param>
		public Table(string name, string partitionName) : this(name, partitionName, new List<Column>())
		{
			this.partitionName = partitionName;
		}
		
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название таблицы</param>
		/// <param name="columns">Столбцы</param>
		private Table(string name, List<Column> columns) : this(name, string.Empty, columns)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название таблицы</param>
		/// <param name="partitionName">Партиция</param>
		/// <param name="columns">Столбцы</param>
		private Table(string name, string partitionName, List<Column> columns) : base(name)
		{
			this.columns = columns;
			this.partitionName = partitionName;
		}

		/// <summary>
		/// Столбцы
		/// </summary>
		public List<Column> Columns
		{
			get
			{
				return columns;
			}
		}

		/// <summary>
		/// Название партиции
		/// </summary>
		public string PartitionName
		{
			get { return partitionName; }
		}

		/// <summary>
		/// Добавляет колонку к таблице
		/// </summary>
		/// <param name="column">Колонка</param>
		/// <returns>Табилца, с добавленой колонкой</returns>
		public Table AddColumn(Column column)
		{
			columns.Add(column);
			return this;
		}

		/// <summary>
		/// Добавляет колонку к таблице
		/// </summary>
		/// <param name="name">Имя столбца</param>
		/// <param name="dbType">DB тип столбца</param>
		/// <param name="nullable">Может содержать null</param>
		/// <param name="identity">Идентити</param>
		/// <returns>Табилца, с добавленой колонкой</returns>
		public Table AddColumn(string name, DbType dbType, bool nullable = true, bool identity = false)
		{
			columns.Add(new Column(name, dbType, nullable, identity));
			return this;
		}

		/// <summary>
		/// Добавляет колонку к таблице
		/// </summary>
		/// <param name="name">Имя столбца</param>
		/// <param name="dbType">DB тип столбца</param>
		/// <param name="nullable">Может содержать null</param>
		/// <param name="identity">Идентити</param>
		/// <returns>Табилца, с добавленой колонкой</returns>
		public Table AddColumn(string name, SqlDbType dbType, bool nullable = true, bool identity = false)
		{
			columns.Add(new Column(name, dbType, nullable, identity));
			return this;
		}

		/// <summary>
		/// Добавляет колонку к таблице
		/// </summary>
		/// <param name="name">Название столбца</param>
		/// <param name="columnType">Тип столбца</param>
		/// <param name="nullable">Может содержать null</param>
		/// <param name="identity">Идентити</param>
		/// <returns>Табилца, с добавленой колонкой</returns>
		public Table AddColumn(string name, ColumnType columnType, bool nullable = true, bool identity = false)
		{
			columns.Add(new Column(name, columnType, nullable, identity));
			return this;
		}

		/// <summary>
		/// Добавляет первичный ключ
		/// </summary>
		/// <param name="columnName">Колонка</param>
		/// <param name="partition">Партиция</param>
		/// <returns>Таблица с первичным ключем</returns>
		public Table AddPrimaryKey(string columnName, string partition = "")
		{
			var column = columns.FirstOrDefault(p => p.Name == columnName);
			if (column == null)
			{
				throw new ColumnExpectedException();
			}

			column.Constraint = new PrimaryKey(string.Format("PK_{0}", Name).ToUpper(), partition);
			return this;
		}

		/// <summary>
		/// Дефалтное значение для последнего столбца
		/// </summary>
		/// <param name="defaultValue">Дефалтное значение</param>
		/// <returns>Текущая таблица</returns>
		public Table Default(string defaultValue)
		{
			Column column = GetColumnForDefault();
			column.Constraint = new ValueDefault(string.Format("DF_{0}_{1}", Name, column.Name).ToUpper(), defaultValue);
			return this;
		}

		/// <summary>
		/// Добавляет дефалт, зависящий от СУБД
		/// </summary>
		/// <param name="standartDefaultType">Дефалт</param>
		/// <returns>Таблица с дефалтом</returns>
		public Table Default(StandartDefaultType standartDefaultType)
		{
			Column column = GetColumnForDefault();
			column.Constraint = new StandartDefault(string.Format("DF_{0}_{1}", Name, column.Name).ToUpper(), standartDefaultType);
			return this;
		}

		/// <summary>
		/// Ищет колонку для дефалта
		/// </summary>
		/// <returns>Колонка</returns>
		private Column GetColumnForDefault()
		{
			if (columns.Count == 0)
			{
				throw new ColumnExpectedException();
			}

			var column = columns.Last();
			if (column.Constraint != null)
			{
				throw new ConflictingDataException("Попытка добавить дефалт два раза");
			}

			return column;
		}
	}
}
