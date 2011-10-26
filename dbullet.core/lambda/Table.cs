﻿using System;
using System.Data;
using System.Linq.Expressions;
using dbullet.core.dbo;

namespace dbullet.core.lambda
{
	/// <summary>
	/// Таблица
	/// </summary>
	/// <typeparam name="T">Тип ДТО</typeparam>
	public class Table<T> : dbo.Table
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		public Table()
			: base(typeof(T).Name)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="partitionName">Партиция</param>
		public Table(string partitionName) : base(typeof(T).Name, partitionName)
		{
		}

		/// <summary>
		/// Добавляет колонку к таблице
		/// </summary>
		/// <param name="name">Имя столбца</param>
		/// <param name="size">Разменость</param>
		/// <param name="nullable">Может содержать null</param>
		/// <param name="identity">Идентити</param>
		/// <typeparam name="P">Тип ДТО</typeparam>
		/// <returns>Табилца, с добавленой колонкой</returns>
		public Table<T> AddColumn<P>(Expression<Func<T, P>> name, int size, bool nullable = true, bool identity = false)
		{
			var m = name.Body as MemberExpression;
			ColumnType ct = null;
			if (m.Type == typeof(int))
			{
				ct = new ColumnType(DbType.Int32);
			}
			else if (m.Type == typeof(string))
			{
				ct = new ColumnType(DbType.String, size);
			}
			
			if (ct == null)
			{
				throw new InvalidExpressionException();
			}

			return (Table<T>)AddColumn(m.Member.Name, ct, nullable, identity);			
		}

		/// <summary>
		/// Добавляет колонку к таблице
		/// </summary>
		/// <param name="name">Имя столбца</param>
		/// <param name="nullable">Может содержать null</param>
		/// <param name="identity">Идентити</param>
		/// <typeparam name="P">Тип ДТО</typeparam>
		/// <returns>Табилца, с добавленой колонкой</returns>
		public Table<T> AddColumn<P>(Expression<Func<T, P>> name, bool nullable = true, bool identity = false)
		{
			return AddColumn(name, 0, nullable, identity);
		}

		/// <summary>
		/// Добавляет первичный ключ
		/// </summary>
		/// <param name="name">Колонка</param>
		/// <param name="partition">Партиция</param>
		/// <typeparam name="P">Тип ДТО</typeparam>
		/// <returns>Таблица с первичным ключем</returns>
		public Table<T> AddPrimaryKey<P>(Expression<Func<T, P>> name, string partition = "PRIMARY")
		{
			var m = name.Body as MemberExpression;
			return (Table<T>)AddPrimaryKey(m.Member.Name, partition);
		}
	}
}