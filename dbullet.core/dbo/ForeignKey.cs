//-----------------------------------------------------------------------
// <copyright file="ForeignKey.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Внешний ключ
	/// </summary>
	public class ForeignKey : DatabaseObjectBase
	{
		/// <summary>
		/// Исходная таблица
		/// </summary>
		private readonly string srcTableName;

		/// <summary>
		/// Колонка исходной таблицы для ключа
		/// </summary>
		private readonly string srcColumnName;

		/// <summary>
		/// Ссылочная таблица
		/// </summary>
		private readonly string refTableName;

		/// <summary>
		/// Колонка ссылочной таблицы для ключа
		/// </summary>
		private readonly string refColumnName;

		/// <summary>
		/// Действие при удалении
		/// </summary>
		private readonly ForeignAction deleteAction;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="srcTableName">Исходная таблица</param>
		/// <param name="srcColumnName">Колонка исходной таблицы для ключа</param>
		/// <param name="refTableName">Ссылочная таблица</param>
		/// <param name="refColumnName">Колонка ссылочной таблицы для ключа</param>
		/// <param name="deleteAction">Действие при удалении</param>
		public ForeignKey(string srcTableName, string srcColumnName, string refTableName, string refColumnName, ForeignAction deleteAction = ForeignAction.SetNull)
			: this(string.Format("FK_{0}_{1}", srcTableName, refTableName), srcTableName, srcColumnName, refTableName, refColumnName, deleteAction)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название ключа</param>
		/// <param name="srcTableName">Исходная таблица</param>
		/// <param name="srcColumnName">Колонка исходной таблицы для ключа</param>
		/// <param name="refTableName">Ссылочная таблица</param>
		/// <param name="refColumnName">Колонка ссылочной таблицы для ключа</param>
		/// <param name="deleteAction">Действие при удалении</param>
		public ForeignKey(string name, string srcTableName, string srcColumnName, string refTableName, string refColumnName, ForeignAction deleteAction = ForeignAction.SetNull)
			: base(name)
		{
			this.srcTableName = srcTableName;
			this.srcColumnName = srcColumnName;
			this.refTableName = refTableName;
			this.refColumnName = refColumnName;
			this.deleteAction = deleteAction;
		}

		/// <summary>
		/// Исходная таблица
		/// </summary>
		public string SrcTableName
		{
			get { return srcTableName; }
		}

		/// <summary>
		/// Колонка исходной таблицы для ключа
		/// </summary>
		public string SrcColumnName
		{
			get { return srcColumnName; }
		}

		/// <summary>
		/// Ссылочная таблица
		/// </summary>
		public string RefTableName
		{
			get { return refTableName; }
		}

		/// <summary>
		/// Колонка ссылочной таблицы для ключа
		/// </summary>
		public string RefColumnName
		{
			get { return refColumnName; }
		}

		/// <summary>
		/// Действие при удалении
		/// </summary>
		public ForeignAction DeleteAction
		{
			get { return deleteAction; }
		}

		/// <summary>
		/// ТуСтринг
		/// </summary>
		/// <returns>Стринг</returns>
		public override string ToString()
		{
			return string.Format("{5}({4}): {0}[{1}] => {2}[{3}]", srcTableName, srcColumnName, refTableName, refColumnName, deleteAction, Name);
		}
	}
}