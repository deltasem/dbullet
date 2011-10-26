using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using dbullet.core.dbo;

namespace dbullet.core.lambda
{
	/// <summary>
	/// Лямбда индексы
	/// </summary>
	/// <typeparam name="T">Тип таблицы</typeparam>
	public class Index<T> : Index
	{
		/// <summary>
		/// Автоматически создавать имя
		/// </summary>
		private bool autoName = false;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название объекта</param>
		/// <param name="partitionName">Партиция</param>
		/// <param name="indexType">Тип индекса</param>
		/// <param name="isUnique">Уникальный индекс</param>
		public Index(string name, string partitionName = "PRIMARY", IndexType indexType = IndexType.Nonclustered, bool isUnique = false) : 
			base(name, typeof(T).Name, partitionName, indexType, isUnique)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="partitionName">Партиция</param>
		/// <param name="indexType">Тип индекса</param>
		/// <param name="isUnique">Уникальный индекс</param>
		public Index(string partitionName = "PRIMARY", IndexType indexType = IndexType.Nonclustered, bool isUnique = false) :
			base(string.Empty, typeof(T).Name, partitionName, indexType, isUnique)
		{
			autoName = true;
		}

		/// <summary>
		/// Добавить колонку к индексу
		/// </summary>
		/// <param name="name">Название объекта</param>
		/// <param name="direction">Направление сортировки</param>
		/// <typeparam name="P">Тип ДТО</typeparam>
		/// <returns>Индекс, с добавленой колонкой</returns>
		public Index<T> AddColumn<P>(Expression<Func<T, P>> name, Direction direction = Direction.Ascending)
		{
			var m = name.Body as MemberExpression;
			var result = (Index<T>)AddColumn(new IndexColumn(m.Member.Name, direction));
			if (autoName)
			{
				StringBuilder sb = new StringBuilder(this.Table.Name);
				foreach (var t in Columns)
				{
					sb.AppendFormat("_{0}", t.Name);
				}

				sb.Append("_idx");
				Name = sb.ToString();
			}

			return result;
		}
	}
}