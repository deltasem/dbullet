//-----------------------------------------------------------------------
// <copyright file="Index.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using dbullet.core.exception;

namespace dbullet.core.dbo
{
	/// <summary>
	/// Индекс
	/// </summary>
	public class Index : DatabaseObjectBase, IPartitionable
	{
		/// <summary>
		/// Тип индекса
		/// </summary>
		private readonly IndexType indexType;

		/// <summary>
		/// Уникальность индекса
		/// </summary>
		private readonly bool isUnique;

		/// <summary>
		/// Название партиции
		/// </summary>
		private readonly string partitionName;

		/// <summary>
		/// Таблица
		/// </summary>
		private readonly Table table;

		/// <summary>
		/// Столбцы для индекса
		/// </summary>
		private readonly List<IndexColumn> columns;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название объекта</param>
		/// <param name="table">Таблица</param>
		/// <param name="partitionName">Партиция</param>
		/// <param name="indexType">Тип индекса</param>
		/// <param name="isUnique">Уникальный индекс</param>
		public Index(string name, string table, string partitionName = "", IndexType indexType = IndexType.Nonclustered, bool isUnique = false)
			: this(name, new Table(table), new IndexColumn[0], partitionName, indexType, isUnique)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название объекта</param>
		/// <param name="table">Таблица</param>
		/// <param name="partitionName">Партиция</param>
		/// <param name="indexType">Тип индекса</param>
		/// <param name="isUnique">Уникальный индекс</param>
		public Index(string name, Table table, string partitionName = "", IndexType indexType = IndexType.Nonclustered, bool isUnique = false)
			: this(name, table, new IndexColumn[0], partitionName, indexType, isUnique)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название индекса</param>
		/// <param name="table">Таблица</param>
		/// <param name="columns">Столбцы для индекса</param>
		/// <param name="partitionName">Партиция</param>
		/// <param name="indexType">Тип индекса</param>
		/// <param name="isUnique">Уникальный индекс</param>
		public Index(string name, string table, IEnumerable<IndexColumn> columns, string partitionName = "", IndexType indexType = IndexType.Nonclustered, bool isUnique = false)
			: this(name, new Table(table), columns, partitionName, indexType, isUnique)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название индекса</param>
		/// <param name="table">Таблица</param>
		/// <param name="columns">Столбцы для индекса</param>
		/// <param name="partitionName">Партиция</param>
		/// <param name="indexType">Тип индекса</param>
		/// <param name="isUnique">Уникальный индекс</param>
		public Index(string name, Table table, IEnumerable<IndexColumn> columns, string partitionName = "", IndexType indexType = IndexType.Nonclustered, bool isUnique = false)
			: base(name)
		{
			this.table = table;
			this.columns = new List<IndexColumn>(columns);
			this.indexType = indexType;
			this.partitionName = partitionName;
			this.isUnique = isUnique;
		}

		/// <summary>
		/// Тип индекса
		/// </summary>
		public IndexType IndexType
		{
			get { return indexType; }
		}

		/// <summary>
		/// Уникальность индекса
		/// </summary>
		public bool IsUnique
		{
			get { return isUnique; }
		}

		/// <summary>
		/// Столбцы для индекса
		/// </summary>
		public List<IndexColumn> Columns
		{
			get { return columns; }
		}

		/// <summary>
		/// Название партиции
		/// </summary>
		public string PartitionName
		{
			get { return partitionName; }
		}

		/// <summary>
		/// Таблица
		/// </summary>
		public Table Table
		{
			get { return table; }
		}

		/// <summary>
		/// Добавить колонку к индексу
		/// </summary>
		/// <param name="name">Название объекта</param>
		/// <param name="direction">Направление сортировки</param>
		/// <returns>Индекс, с добавленой колонкой</returns>
		public Index AddColumn(string name, Direction direction = Direction.Ascending)
		{
			return AddColumn(new IndexColumn(name, direction));
		}

		/// <summary>
		/// Добавить колонку к индексу
		/// </summary>
		/// <param name="column">Колонка</param>
		/// <returns>Индекс, с добавленой колонкой</returns>
		public Index AddColumn(IndexColumn column)
		{
			if (columns.FirstOrDefault(p => p.Name == column.Name) != null)
			{
				throw new DublicateColumnException();
			}

			columns.Add(column);
			return this;
		}
	}
}